using System.Net.Http.Json;
using ApiContracts;

namespace DnpBlazorClient.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("users", request);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        HttpResponseMessage response =
            await client.PatchAsJsonAsync($"users/{id}", request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        return (await client.GetFromJsonAsync<UserDto>($"users/{id}"))!;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        return (await client.GetFromJsonAsync<List<UserDto>>("users"))!;
    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage response =
            await client.DeleteAsync($"users/{id}");

        response.EnsureSuccessStatusCode();
    }
}