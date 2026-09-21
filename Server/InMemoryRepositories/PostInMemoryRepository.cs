using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private readonly List<Post> posts = [];
    
    public PostInMemoryRepository()
    {
        _ = AddAsync(new Post(
            "What is your favorite coffee?",
            "I usually drink cappuccino, but I would love to try something new.",
            1)).Result;

        _ = AddAsync(new Post(
            "Best milk for a latte?",
            "Do you prefer regular milk, oat milk, or something else in your latte?",
            2)).Result;

        _ = AddAsync(new Post(
            "Coffee for studying",
            "What coffee would you recommend for a long study session?",
            3)).Result;

        _ = AddAsync(new Post(
            "Espresso vs Americano",
            "Which one do you prefer and why?",
            1)).Result;
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }
    
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post=posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
           throw new InvalidOperationException($"Post with ID '{id}' not found"); 
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}