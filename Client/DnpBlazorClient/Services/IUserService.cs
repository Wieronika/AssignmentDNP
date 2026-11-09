using ApiContracts;

namespace DnpBlazorClient.Services;

// Defines the contract for user-related operations in Blazor.
// Specifies the methods that must be implemented by HttpUserService.
public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task UpdateUserAsync(int id, UpdateUserDto request);
    Task<UserDto> GetUserAsync(int id);
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task DeleteUserAsync(int id);
}