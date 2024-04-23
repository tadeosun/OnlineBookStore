using EBookStore.Data;
using EBookStore.Models;
using EBookStore.RepositoryInterfaces;
using EBookStore.ResponseDto;
using Microsoft.EntityFrameworkCore;

namespace EBookStore.RepositoryImplementation
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly DataContext _context;

        public CheckoutRepository(DataContext context)
        {
            _context = context;
        }
   
        public async Task AddOrderItemsAsync(OrderItemDetails orderItemDetail)
        {
            _context.OrderItemDetails.Add(orderItemDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddOrdersAsync(Orders order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Orders>> GetOrderByUsernameAsync(string username)
        {
            return await _context.Orders.Where(u => u.Username == username).ToListAsync();
        }


        public async Task BulkInsertOrderDetailItemssAsync(List<OrderItemDetails> orderItemDetails)
        {
            _context.OrderItemDetails.AddRange(orderItemDetails);
            await _context.SaveChangesAsync();
        }

        public async Task<Orders> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        public async Task<List<OrderItemDetails>> GetOrderItemDetailsByIdAsync(int id)
        {
            return await _context.OrderItemDetails.Where(a => a.OrderID == id).ToListAsync();

        }
    }
}
