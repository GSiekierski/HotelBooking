using Microsoft.AspNetCore.Mvc;
using HotelBookingAPI.Models;
using HotelBookingAPI.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly JwtService _jwtService;

    public AuthController(AuthService authService, JwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }


    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        var newUser = _authService.Register(user.Username, user.Password);
        if (newUser == null)
            return BadRequest("User already exists");

        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {

        var existingUser = _authService.Authenticate(user.Username, user.Password);
        if (existingUser == null)
            return Unauthorized();

        var token = _jwtService.GenerateToken(user.Username);
        return Ok(new { token });
    }
}
