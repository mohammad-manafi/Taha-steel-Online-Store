using System.Collections.Generic;
using ShopManagement.Application.Contracts.Order;

namespace OnlineStoreQuery.Contracts
{
    public interface ICartCalculatorService
    {
        Cart ComputeCart(List<CartItem> cartItems);
    }
}