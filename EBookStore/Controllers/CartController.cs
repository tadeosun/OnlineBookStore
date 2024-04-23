using EBookStore.Common;
using EBookStore.Interfaces;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace EBookStore.Controllers
{
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("Cart/api/ViewCart")]
        [Authorize]
        public async Task<IActionResult> ViewCart()
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _cartService.ViewCart(loggedinUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("Cart/api/CreateCart")]
        [Authorize]
        public async Task<IActionResult> CreateCart([FromBody] ShoppingCartRequest request)
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _cartService.CreateCart(request, loggedinUser);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Cart/api/RemoveItem")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _cartService.RemoveItemFromCart(id, loggedinUser);
            return Ok(result);
        }

    }
}
