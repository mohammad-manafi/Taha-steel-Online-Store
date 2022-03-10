using System.Collections.Generic;
using _0_Framework.Application;
using _0_Framework.Application.Sms;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IAuthHelper authHelper;
        private readonly IConfiguration configuration;
        private readonly IOrderRepository orderRepository;
        private readonly IShopInventoryAcl shopInventoryAcl;
        private readonly ISmsService smsService;
        private readonly IShopAccountAcl shopAccountAcl;

        public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration,
            IShopInventoryAcl shopInventoryAcl, ISmsService smsService, IShopAccountAcl shopAccountAcl)
        {
            this.orderRepository = orderRepository;
            this.authHelper = authHelper;
            this.configuration = configuration;
            this.shopInventoryAcl = shopInventoryAcl;
            this.smsService = smsService;
            this.shopAccountAcl = shopAccountAcl;
        }

        public long PlaceOrder(Cart cart)
        {
            var currentAccountId = authHelper.CurrentAccountId();
            var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
                cart.PayAmount);

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem(cartItem.Id, cartItem.Count, cartItem.UnitPrice, cartItem.DiscountRate);
                order.AddItem(orderItem);
            }

            orderRepository.Create(order);
            orderRepository.SaveChanges();
            return order.Id;
        }

        public double GetAmountBy(long id)
        {
            return orderRepository.GetAmountBy(id);
        }

        public void Cancel(long id)
        {
            var order = orderRepository.Get(id);
            order.Cancel();
            orderRepository.SaveChanges();
        }

        public string PaymentSucceeded(long orderId, long refId)
        {
            //var order = orderRepository.Get(orderId);
            //order.PaymentSucceeded(refId);
            //var symbol = configuration.GetValue<string>("Symbol");
            //var issueTrackingNo = CodeGenerator.Generate(symbol);
            //order.SetIssueTrackingNo(issueTrackingNo);
            //if (!shopInventoryAcl.ReduceFromInventory(order.Items)) return "";

            //orderRepository.SaveChanges();

            //var (name, mobile) = shopAccountAcl.GetAccountBy(order.AccountId);

            //smsService.Send(mobile,
            //    $"{name} گرامی سفارش شما با شماره پیگیری {issueTrackingNo} با موفقیت پرداخت شد و ارسال خواهد شد.");
            //return issueTrackingNo;

            throw new System.NotImplementedException();
        }

        public List<OrderItemViewModel> GetItems(long orderId)
        {
            return orderRepository.GetItems(orderId);
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            return orderRepository.Search(searchModel);
        }
        
    }
}