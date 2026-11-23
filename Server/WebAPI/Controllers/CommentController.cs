
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(
        ICommentRepository commentRepo,
        IPostRepository postRepo,
        IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetComment([FromRoute] int id)
    {
        Comment? comment = await commentRepo.GetMany()
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return Results.NotFound($"Comment with ID {id} not found.");
        }

        CommentDto dto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            UserName = comment.User.UserName,
            PostId = comment.PostId
        };

        return Results.Ok(dto);
    }

    [HttpGet("/posts/{postId:int}/comments")]
    public async Task<IResult> GetCommentsForPost(
        [FromRoute] int postId)
    {
        if (!await postRepo.GetMany().AnyAsync(p => p.Id == postId))
        {
            return Results.NotFound($"Post with ID {postId} not found.");
        }

        List<CommentDto> dtos = await commentRepo.GetMany()
            .Where(comment => comment.PostId == postId)
            .Select(comment => new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId,
                UserName = comment.User.UserName
            })
            .ToListAsync();

        return Results.Ok(dtos);
    }

    [HttpPost("/posts/{postId:int}/comments")]
    public async Task<IResult> CreateComment(
        [FromRoute] int postId,
        [FromBody] CreateCommentDto request)
    {
        try
        {
            await postRepo.GetSingleAsync(postId);
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"Post with ID {postId} not found.");
        }

        User user;

        try
        {
            user = await userRepo.GetSingleAsync(request.UserId);
        }
        catch (InvalidOperationException)
        {
            return Results.BadRequest("User does not exist.");
        }

        Comment comment = new Comment(
            request.Body,
            request.UserId,
            postId);

        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = new CommentDto
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            UserName = user.UserName,
            PostId = created.PostId
        };

        return Results.Created($"/comments/{created.Id}", dto);
    }

    [HttpPatch("{id:int}")]
    public async Task<IResult> UpdateComment(
        [FromRoute] int id,
        [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);

            comment.Body = request.Body;

            await commentRepo.UpdateAsync(comment);

            return Results.NoContent();
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"Comment with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);

            return Results.NoContent();
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"Comment with ID {id} not found.");
        }
    }
    [HttpGet]
    public async Task<IResult> GetComments(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> comments = commentRepo.GetMany();

        if (userId != null)
        {
            comments = comments.Where(c => c.UserId == userId);
        }

        if (postId != null)
        {
            comments = comments.Where(c => c.PostId == postId);
        }

        List<CommentDto> dtos = await comments
            .Select(comment => new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId,
                UserName = comment.User.UserName
            })
            .ToListAsync();

        return Results.Ok(dtos);
    } 
}
