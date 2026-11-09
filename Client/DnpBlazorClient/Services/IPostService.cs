using ApiContracts;

namespace DnpBlazorClient.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);

    Task UpdatePostAsync(int id, UpdatePostDto request);

    Task<PostDto> GetPostAsync(int id);

    Task<IEnumerable<PostDto>> GetPostsAsync(
        int? userId = null,
        string? title = null);

    Task DeletePostAsync(int id);
}