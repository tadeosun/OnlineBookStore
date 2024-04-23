using EBookStore.Common;
using EBookStore.Interfaces;
using EBookStore.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBookStore.Controllers
{
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InventoryController(IInventoryService inventoryService, IHttpContextAccessor httpContextAccessor)
        {
            _inventoryService = inventoryService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("Inventory/api/GetInventory")]
        public async Task<IActionResult> GetInventory(int id)
        {
            var result = await _inventoryService.GetInventory(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("Inventory/api/CreateInventory")]
        [Authorize]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryRequestDto request)
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _inventoryService.CreateInventory(request, loggedinUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("Inventory/api/UpdateInventory")]
        [Authorize]
        public async Task<IActionResult> UpdateInventory([FromBody] InventoryRequestDto request)
        {
            var loggedinUser = Helper.GetLoggedInUser(_httpContextAccessor);
            var result = await _inventoryService.UpdateInventory(request, loggedinUser);
            return Ok(result);
        }

        [HttpGet]
        [Route("Inventory/api/SearchInventory")]
        [Authorize]
        public async Task<IActionResult> SearchInventory(string searchTerm)
        {
            var result = await _inventoryService.SearchInventory(searchTerm);
            return Ok(result);
        }


        [HttpGet]
        [Route("Inventory/api/GetAllInventory")]
        public async Task<IActionResult> GetAllInventory()
        {
            var result = await _inventoryService.GetAllInventoryAsync();
            return Ok(result);
        }
    }

}
