using System.Net.Http.Json;
using ApiContracts;

namespace DnpBlazorClient.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("posts", request);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<PostDto>())!;
    }

    public async Task UpdatePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage response =
            await client.PatchAsJsonAsync($"posts/{id}", request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        return (await client.GetFromJsonAsync<PostDto>($"posts/{id}"))!;
    }

    public async Task<IEnumerable<PostDto>> GetPostsAsync(
        int? userId = null,
        string? title = null)
    {
        return (await client.GetFromJsonAsync<List<PostDto>>("posts"))!;
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage response =
            await client.DeleteAsync($"posts/{id}");

        response.EnsureSuccessStatusCode();
    }
}