using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace EBookStore.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            //try
            //{
                var user = await _authService.AuthenticateUsers(request);
                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                var token = await _authService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Something Went Wrong");
            //}
            
        }
    }
}
