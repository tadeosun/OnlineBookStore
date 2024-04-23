using EBookStore.Common;
using EBookStore.Data;
using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;
using Microsoft.Extensions.Caching.Memory;

namespace EBookStore.Implementations
{
    public class CartService :ICartService
    {
        private readonly IMemoryCache _cache;
        private readonly IDataContext _context;
        public CartService(IMemoryCache cache, IDataContext context) 
        {
            _cache = cache;
            _context = context;
        }

        public async Task<ShoppingCart> CreateCart(ShoppingCartRequest request, string loggedinUser)
        {
            try
            {
                var createOrder = new ShoppingCart();
                var bookToAdd = new Books();
                long id = request.ItemID;
                bookToAdd = await _context.Books.FindAsync(id);

                string cacheKey = string.Format("ItemID-{0}", request.ItemID);
                var ShoppingCartItemList = _cache.Get(cacheKey) as List<ShoppingCartItem> ?? new List<ShoppingCartItem>();

                var existingorder = ShoppingCartItemList.FirstOrDefault(item => item.Book.ID == request.ItemID);
                if (existingorder != null)
                {
                    var addToQty = existingorder.Quantity + 1;
                    ShoppingCartItemList.ForEach(item => item.Quantity = addToQty);
                }
                else
                {
                    ShoppingCartItemList.Add(new ShoppingCartItem
                    {
                        Book = bookToAdd,
                        Quantity = 1
                    });
                }

                var absoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(6));
                _cache.Set(cacheKey, ShoppingCartItemList, absoluteExpiration);

                ShoppingCart cart = GetCart(ShoppingCartItemList, loggedinUser);
                /****No need to insert in DB, Ideally keep in memory using redis cache
                 * for instance to achive a faster through put****/

                createOrder = cart;
                return createOrder;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ShoppingCart> RemoveItemFromCart(int id, string loggedInUser)
        {
            try
            {
                var cart = new ShoppingCart();
                string cacheKey = string.Format("ItemID-{0}", id);
                var ShoppingCartItemList = _cache.Get(cacheKey) as List<ShoppingCartItem> ?? new List<ShoppingCartItem>();

                var itemToRemove = ShoppingCartItemList.FirstOrDefault(item => item.Book.ID == id);
                if (itemToRemove != null)
                {
                    if (itemToRemove.Quantity > 1)
                    {
                        ShoppingCartItemList.ForEach(item => item.Quantity = itemToRemove.Quantity - 1);
                    }
                    else
                    {
                        ShoppingCartItemList.Remove(itemToRemove);
                    }
                }
                _cache.Set(cacheKey, ShoppingCartItemList);
                cart = GetCart(ShoppingCartItemList, loggedInUser);
                /****No need to update in DB, Ideally keep in memory using redis cache for instance****/

                return cart;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ShoppingCart> ViewCart(string username)
        {
            try
            {
                var viewOrder = new ShoppingCart();
                string cacheKey = string.Format("UserCart-{0}", username);
                var shoppingCart = _cache.Get(cacheKey) as ShoppingCart ?? new ShoppingCart();
                //implement using cache on get cart to make view faster
                if (shoppingCart == null)
                {
                    shoppingCart = GetCart(username);

                    if (shoppingCart != null)
                    {
                        var absoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(6));
                        _cache.Set(cacheKey, shoppingCart, absoluteExpiration);
                    }
                }

                viewOrder = shoppingCart;
                return viewOrder;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ShoppingCart GetCart(List<ShoppingCartItem> shoppingCartItemList, string username)
        {
            var cart = new ShoppingCart
            {
                CartItems = shoppingCartItemList,
                TotalPrice = shoppingCartItemList.Sum(item => item.Book.Price * item.Quantity),
                TotalQuantity = shoppingCartItemList.Where(a => a.Book.Quantity > 0).Count()
            };
            _cache.Set(string.Format("UserCart-{0}", username), cart);

            return cart;
        }

        private ShoppingCart GetCart(string username)
        {
            string cacheKey = string.Format("UserCart-{0}", username);
            var shoppingCart = _cache.Get(cacheKey) as ShoppingCart ?? null;
            return shoppingCart;
        }
    }
}
