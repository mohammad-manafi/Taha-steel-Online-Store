using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Infrastructure.EFCore;
using OnlineStoreQuery.Contracts;
using ShopManagement.Application.Contracts.Order;

namespace OnlineStoreQuery.Query
{
    public class CartCalculatorService : ICartCalculatorService
    {
        private readonly IAuthHelper authHelper;
        private readonly DiscountContext discountContext;

        public CartCalculatorService(DiscountContext discountContext, IAuthHelper authHelper)
        {
            this.discountContext = discountContext;
            this.authHelper = authHelper;
        }

        public Cart ComputeCart(List<CartItem> cartItems)
        {
            var cart = new Cart();
            var colleagueDiscounts = discountContext.ColleagueDiscounts
                .Where(x => !x.IsRemved)
                .Select(x => new { x.DiscountRate, x.ProductId })
                .ToList();

            var customerDiscounts = discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.DiscountRate, x.ProductId })
                .ToList();
            var currentAccountRole = authHelper.CurrentAccountRole();

            foreach (var cartItem in cartItems)
            {
                if (currentAccountRole == Roles.ColleagueUser)
                {
                    var colleagueDiscount = colleagueDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                    if (colleagueDiscount != null)
                        cartItem.DiscountRate = colleagueDiscount.DiscountRate;
                }
                else
                {
                    var customerDiscount = customerDiscounts.FirstOrDefault(x => x.ProductId == cartItem.Id);
                    if (customerDiscount != null)
                        cartItem.DiscountRate = customerDiscount.DiscountRate;
                }

                cartItem.DiscountAmount = cartItem.TotalItemPrice * cartItem.DiscountRate / 100;
                cartItem.ItemPayAmount = cartItem.TotalItemPrice - cartItem.DiscountAmount;
                cart.Add(cartItem);
            }

            return cart;
        }
    }
}