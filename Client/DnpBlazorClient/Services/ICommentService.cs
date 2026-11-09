using ApiContracts;

namespace DnpBlazorClient.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(int postId, CreateCommentDto request);

    Task<CommentDto> GetCommentAsync(int id);

    Task<IEnumerable<CommentDto>> GetCommentsForPostAsync(int postId);

    Task UpdateCommentAsync(int id, UpdateCommentDto request);

    Task DeleteCommentAsync(int id);
}