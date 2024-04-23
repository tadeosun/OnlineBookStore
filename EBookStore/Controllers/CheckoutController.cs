using EBookStore.Common;
using EBookStore.Interfaces;
using EBookStore.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBookStore.Controllers
{
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutController(ICheckoutService checkoutService, IHttpContextAccessor httpContextAccessor)
        {
            _checkoutService = checkoutService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        [Route("Checkout/api/SubmitOrder")]
        [Authorize]
        public async Task<IActionResult> SubmitOrder([FromBody] CheckOutRequestDto request)
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _checkoutService.CheckOut(request,loggedinUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("Checkout/api/PurchaseHistory")]
        [Authorize]
        public async Task<IActionResult> PurchaseHistory()
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _checkoutService.ViewPurchaseHistory(loggedinUser);
            return Ok(result);
        }
    }
}
