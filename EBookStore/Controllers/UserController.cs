using EBookStore.Common;
using EBookStore.Implementations;
using EBookStore.Interfaces;
using EBookStore.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBookStore.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("User/api/GetUserById")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("User/api/AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDto request)
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _userService.AddUser(request, loggedinUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("User/api/UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequestDto request)
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _userService.UpdateUser(request, loggedinUser);
            return Ok(result);
        }
    }
}
