using System.Net.Http.Json;
using ApiContracts;

namespace DnpBlazorClient.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(
        int postId, CreateCommentDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"posts/{postId}/comments", request);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<CommentDto>())!;
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        return (await client.GetFromJsonAsync<CommentDto>(
            $"comments/{id}"))!;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsForPostAsync(int postId)
    {
        return (await client.GetFromJsonAsync<List<CommentDto>>(
            $"posts/{postId}/comments"))!;
    }

    public async Task UpdateCommentAsync(
        int id, UpdateCommentDto request)
    {
        HttpResponseMessage response =
            await client.PatchAsJsonAsync($"comments/{id}", request);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage response =
            await client.DeleteAsync($"comments/{id}");

        response.EnsureSuccessStatusCode();
    }
}