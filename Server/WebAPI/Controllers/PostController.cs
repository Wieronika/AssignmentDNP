
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(
        IPostRepository postRepo,
        IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IResult> GetPost([FromRoute] int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            User user = await userRepo.GetSingleAsync(post.UserId);

            PostDto dto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                UserName = user.UserName
            };

            return Results.Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound("Post or author not found.");
        }
    }
    

    [HttpGet]
    public Task<IResult> GetPosts(
        [FromQuery] int? userId,
        [FromQuery] string? title)
    {
        IQueryable<Post> posts = postRepo.GetMany();

        //here I filter posts by author ID
        if (userId != null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        //and here I filter posts by title
        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p =>
                p.Title.Contains(
                    title,
                    StringComparison.OrdinalIgnoreCase));
        }

        List<PostDto> dtos = posts.Select(post => new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            UserName = userRepo.GetMany()
                .Where(user => user.Id == post.UserId)
                .Select(user => user.UserName)
                .FirstOrDefault() ?? ""
        }).ToList();

        return Task.FromResult<IResult>(Results.Ok(dtos));
    }

    
    [HttpPost]
    public async Task<IResult> CreatePost(
        [FromBody] CreatePostDto request)
    {
        try
        {
            await userRepo.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException)
        {
            return Results.BadRequest("User does not exist.");
        }

        Post post = new Post(
            request.Title,
            request.Body,
            request.UserId);

        Post created = await postRepo.AddAsync(post);

        User author = await userRepo.GetSingleAsync(created.UserId);

        PostDto dto = new PostDto
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId,
            UserName = author.UserName
        };

        return Results.Created($"/posts/{created.Id}", dto);
    }
    
    [HttpPatch("{id:int}")]
    public async Task<IResult> UpdatePost(
        [FromRoute] int id,
        [FromBody] UpdatePostDto request)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);

            post.Title = request.Title;
            post.Body = request.Body;

            await postRepo.UpdateAsync(post);

            return Results.NoContent();
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"Post with ID {id} not found.");
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);

            return Results.NoContent();
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"Post with ID {id} not found.");
        }
    }
}
