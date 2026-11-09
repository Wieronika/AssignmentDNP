using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public IResult Login([FromBody] LoginRequest request)
    {
        User? user = userRepo.GetMany()
            .FirstOrDefault(u => u.UserName == request.UserName);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        if (user.Password != request.Password)
        {
            return Results.Unauthorized();
        }

        UserDto dto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Results.Ok(dto);
    }
}