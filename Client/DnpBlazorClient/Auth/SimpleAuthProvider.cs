
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace DnpBlazorClient.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            // Fora: LoginRequest uses properties instead of a constructor.
            new LoginRequest
            {
                UserName = userName,
                Password = password
            });

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Invalid username or password.");
        }

        // Fora: Use the same JSON method as our other HTTP services.
        UserDto? userDto = await response.Content.ReadFromJsonAsync<UserDto>();

        if (userDto == null)
        {
            throw new Exception("Could not read user information.");
        }

        string serialisedData = JsonSerializer.Serialize(userDto);

        await jsRuntime.InvokeVoidAsync(
            "sessionStorage.setItem",
            "currentUser",
            serialisedData
        );

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(claimsPrincipal))
        );
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? userAsJson;

        try
        {
            userAsJson = await jsRuntime.InvokeAsync<string?>(
                "sessionStorage.getItem",
                "currentUser"
            );
        }
        catch (InvalidOperationException)
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        UserDto? userDto = JsonSerializer.Deserialize<UserDto>(userAsJson);

        if (userDto == null)
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        return new AuthenticationState(claimsPrincipal);
    }

    public async Task Logout()
    {
        await jsRuntime.InvokeVoidAsync(
            "sessionStorage.setItem",
            "currentUser",
            ""
        );

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal())
            )
        );
    }
}
