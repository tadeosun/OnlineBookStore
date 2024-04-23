using EBookStore.Common;
using EBookStore.Interfaces;
using EBookStore.Models;
using EBookStore.RepositoryInterfaces;
using EBookStore.RequestDto;
using EBookStore.ResponseDto;
using Microsoft.Data.SqlClient;
using System.Transactions;

namespace EBookStore.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IConfigurationAccessor _configAccessor;

        public CheckoutService(ICheckoutRepository checkoutRepository, IInventoryRepository inventoryRepository, IConfigurationAccessor configAccessor)
        {
            _checkoutRepository = checkoutRepository;
            _inventoryRepository = inventoryRepository;
            _configAccessor = configAccessor;
        }

        public async Task<Puchasehistory> CheckOut(CheckOutRequestDto request, string loggedinUser)
        {
            try
            {
                var response = new Puchasehistory();

                //validate request payload
                if (request == null || request.Cart == null
                    || request.Cart.CartItems.Count <= 0
                   || (string.IsNullOrEmpty(request.PaymentOption)))
                {
                    response.ResponseCode = ResponseMapping.ResponseCode02;
                    response.ResponseMessage = ResponseMapping.ResponseCode02Message;
                    return response;
                }

                // Check if items are out of stock or price has changed
                var cartItems = request.Cart.CartItems;
                foreach (var item in cartItems)
                {
                    var product = await _inventoryRepository.GetInventoryByIdAsync((int)item.Book.ID);
                    if (product == null)
                    {
                        // Product not found, remove it from the cart
                        cartItems.Remove(item);
                        //continue;
                    }

                    if (product.Quantity < item.Quantity || product.Price != item.Book.Price)
                    {
                        // Product out of stock or price has changed, remove it from the cart
                        cartItems.Remove(item);
                    }
                }

                // If cart is empty after invalidating items, return error
                if (cartItems.Count == 0)
                {
                    response.ResponseCode = ResponseMapping.ResponseCode08;
                    response.ResponseMessage = ResponseMapping.ResponseCode08Message;
                    return response;
                }

                // Simulate payment process
                var paymentId = GeneratePaymentId();
                await SimulatePaymentProcessAsync(paymentId, request.PaymentOption);


                // Create order
                var order = new Orders
                {
                    Username = loggedinUser,
                    TotalQuantity = request.Cart.TotalQuantity,
                    TotalAmount = request.Cart.TotalPrice,
                    IsPaid = true, // Simulate payment success
                    DateOrdered = DateTime.Now
                };

                using (var transactionScope = new TransactionScope())
                {
                    try
                    {
                        // Save order to database or perform other operations
                        var id = await _checkoutRepository.AddOrdersAsync(order);

                        //Create OrderItemdetails
                        List<OrderItemDetails> itemdetailsList = CreateRecordtoInsert(cartItems, id);
                        if (itemdetailsList.Count > 0)
                        {
                            await _checkoutRepository.BulkInsertOrderDetailItemssAsync(itemdetailsList);
                        }
                        // Commit the transaction
                        transactionScope.Complete();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction
                        transactionScope.Dispose();
                        throw;
                    }
                }
                // Invalidate the cart by removing the items
                foreach (var item in cartItems)
                {
                    if (item != null)
                    {
                        cartItems.Remove(item);
                    }
                }

                response.ResponseCode = ResponseMapping.ResponseCode00;
                response.ResponseMessage = ResponseMapping.ResponseCode00Message;
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GeneratePaymentId()
        {
            // Simulate generating a payment ID
            return Guid.NewGuid().ToString();
        }

        private async Task SimulatePaymentProcessAsync(string paymentId, string paymentOption)
        {
            // Simulate payment processing delay
            await Task.Delay(2000);
        }

        private List<OrderItemDetails> CreateRecordtoInsert(List<ShoppingCartItem> cartItems, long orderID)
        {
            var itemdetailsList = new List<OrderItemDetails>();

            for (var i = 0; i < cartItems.Count; i++)
            {
                var itemDetails = new OrderItemDetails()
                {
                    BookID = cartItems[i].Book.ID,
                    Price = cartItems[i].Book.Price,
                    Quantity = cartItems[i].Book.Quantity,
                    OrderID = orderID,
                    OrderDateTime = DateTime.Now
                };

                itemdetailsList.Add(itemDetails);
            }
            return itemdetailsList;
        }


        public async Task<IList<Puchasehistory>> ViewPurchaseHistory(string username)
        {
            try
            {
                var purchaseHistoryInfo = new List<Orders>();
                var orderitemList = new List<Puchasehistory>();

                purchaseHistoryInfo = await _checkoutRepository.GetOrderByUsernameAsync(username);

                foreach (var item in purchaseHistoryInfo)
                {
                    var result = await _checkoutRepository.GetOrderItemDetailsByIdAsync((int)item.ID);

                    for (var i = 0; i < result.Count; i++)
                    {
                        var book = await _inventoryRepository.GetInventoryByIdAsync(result[i].BookID);

                        var cartItem = new Puchasehistory()
                        {
                            Quantity = result[i].Quantity,
                            Price = result[i].Price,
                            BookDetails = book,
                            DateOrdered = result[i].OrderDateTime,
                            OrderID = result[i].OrderID
                        };

                        orderitemList.Add(cartItem);
                    }
                }

                return orderitemList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
