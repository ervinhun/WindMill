using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WindMill.DataAccess;
using WindMill.Util;

namespace WindMill.Controller;

[ApiController]
[Route("api/[controller]")]
public class LoginController(MyDbContext ctx, IConfiguration config, JwtService jwt) : ControllerBase
{
    [HttpPost(nameof(Login))]
    [Produces<LoginResponseDto>]
    public async Task<LoginResponseDto> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new ValidationException("Username and password are required.");

        var pepper = config["SECRET"]?.Substring(0, 16);
        var user = await ctx.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsDeleted == false);
        if (user == null || !GenerateHashPass.Verify(request.Password, user.Password, pepper))
            throw new ValidationException("Username or password is incorrect");
        return ConvertUserToLoginResponse(user);
    }


    private LoginResponseDto ConvertUserToLoginResponse(User user)
    {
        var token = jwt.GenerateToken(user.Id.ToString(), user.Username, user.Role.Name);
        return new LoginResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(24),
            UserName = user.Username,
            RoleName = user.Role.Name
        };
    }

    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}