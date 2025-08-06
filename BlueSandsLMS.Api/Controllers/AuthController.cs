
using BlueSandsLMS.Application.Services;
using BlueSandsLMS.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlueSandsLMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        // Public student self-registration
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try { return Ok(await _auth.RegisterAsync(dto)); }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        // Public login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try { return Ok(await _auth.LoginAsync(dto)); }
            catch (Exception ex) { return Unauthorized(new { message = ex.Message }); }
        }
    }
}
