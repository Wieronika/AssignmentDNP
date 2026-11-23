
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetUser([FromRoute] int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);

            UserDto dto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return Results.Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"User with ID {id} not found.");
        }
    }

    [HttpGet]
    public async Task<IResult> GetUsers([FromQuery] string? userName)
    {
        IQueryable<User> users = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            string search = userName.ToLower();

            users = users.Where(u =>
                u.UserName.ToLower().Contains(search));
        }

        List<UserDto> dtos = await users.Select(user => new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        }).ToListAsync();

        return Results.Ok(dtos);
    }

    [HttpPost]
    public async Task<IResult> CreateUser([FromBody] CreateUserDto request)
    {
        User user = new User(request.UserName, request.Password);

        User created = await userRepo.AddAsync(user);

        UserDto dto = new UserDto
        {
            Id = created.Id,
            UserName = created.UserName
        };

        return Results.Created($"/users/{created.Id}", dto);
    }

    [HttpPatch("{id:int}")]
    public async Task<IResult> UpdateUser(
        [FromRoute] int id,
        [FromBody] UpdateUserDto request)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);

            user.UserName = request.UserName;
            user.Password = request.Password;

            await userRepo.UpdateAsync(user);

            return Results.NoContent();
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"User with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);

            return Results.NoContent();
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound($"User with ID {id} not found.");
        }
    }
}
