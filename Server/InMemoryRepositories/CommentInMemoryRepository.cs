using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> comments = [];
    
    public CommentInMemoryRepository()
    {
        // Comments for Post 1
        _ = AddAsync(new Comment(
            "You should definitely try a flat white!",
            2,
            1)).Result;

        _ = AddAsync(new Comment(
            "I really like iced lattes.",
            3,
            1)).Result;

        // Comments for Post 2
        _ = AddAsync(new Comment(
            "Oat milk is my favorite, especially with darker coffee.",
            1,
            2)).Result;

        _ = AddAsync(new Comment(
            "I still prefer regular milk in my latte.",
            3,
            2)).Result;

        // Comments for Post 3
        _ = AddAsync(new Comment(
            "Double espresso always gets me through long study sessions!",
            1,
            3)).Result;

        _ = AddAsync(new Comment(
            "Cold brew is also a good option and tastes less bitter.",
            2,
            3)).Result;

        _ = AddAsync(new Comment(
            "I usually go for an Americano when I need to study.",
            3,
            3)).Result;

        // Comments for Post 4
        _ = AddAsync(new Comment(
            "Espresso for me! Small but strong.",
            3,
            4)).Result;

        _ = AddAsync(new Comment(
            "Americano, I like having something to sip for longer.",
            2,
            4)).Result;
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;

        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);

        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}