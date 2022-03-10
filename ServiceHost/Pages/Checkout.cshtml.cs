using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Application.ZarinPal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using OnlineStoreQuery.Contracts;
using OnlineStoreQuery.Contracts.Product;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        public Cart Cart;
        public const string CookieName = "cart-items";
        private readonly IAuthHelper authHelper;
        private readonly ICartService cartService;
        private readonly IProductQuery productQuery;
        private readonly IZarinPalFactory zarinPalFactory;
        private readonly IOrderApplication orderApplication;
        private readonly ICartCalculatorService cartCalculatorService;

        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService,
            IProductQuery productQuery, IOrderApplication orderApplication, IZarinPalFactory zarinPalFactory,
            IAuthHelper authHelper)
        {
            Cart = new Cart();
           this.cartCalculatorService = cartCalculatorService;
           this.cartService = cartService;
           this.productQuery = productQuery;
           this.orderApplication = orderApplication;
           this.zarinPalFactory = zarinPalFactory;
           this.authHelper = authHelper;
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = cartCalculatorService.ComputeCart(cartItems);
            cartService.Set(Cart);
        }

        public IActionResult OnPostPay(int paymentMethod)
        {
            var cart = cartService.Get();
            cart.SetPaymentMethod(paymentMethod);

            var result = productQuery.CheckInventoryStatus(cart.Items);
            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("/Cart");

            var orderId = orderApplication.PlaceOrder(cart);
            if (paymentMethod == 1)
            {
                var paymentResponse = zarinPalFactory.CreatePaymentRequest(
                    cart.PayAmount.ToString(CultureInfo.InvariantCulture), "", "",
                    "خرید از درگاه لوازم خانگی و دکوری", orderId);

                return Redirect(
                    $"https://{zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
            }

            var paymentResult = new PaymentResult();
            return RedirectToPage("/PaymentResult",
                paymentResult.Succeeded(
                    "سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما و پرداخت وجه، سفارش ارسال خواهد شد.", null));
        }

        public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status,
            [FromQuery] long oId)
        {
            var orderAmount = orderApplication.GetAmountBy(oId);
            var verificationResponse =
                zarinPalFactory.CreateVerificationRequest(authority,
                    orderAmount.ToString(CultureInfo.InvariantCulture));

            var result = new PaymentResult();
            if (status == "OK" && verificationResponse.Status >= 100)
            {
                var issueTrackingNo = orderApplication.PaymentSucceeded(oId, verificationResponse.RefID);
                Response.Cookies.Delete("cart-items");
                result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
                return RedirectToPage("/PaymentResult", result);
            }

            result = result.Failed(
                "پرداخت با موفقیت انجام نشد. درصورت کسر وجه از حساب، مبلغ تا 24 ساعت دیگر به حساب شما بازگردانده خواهد شد.");
            return RedirectToPage("/PaymentResult", result);
        }
    }
}