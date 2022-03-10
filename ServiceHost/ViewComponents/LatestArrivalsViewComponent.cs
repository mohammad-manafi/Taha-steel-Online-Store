using Microsoft.AspNetCore.Mvc;
using OnlineStoreQuery.Contracts.Product;

namespace ServiceHost.ViewComponents
{
    public class LatestArrivalsViewComponent : ViewComponent
    {
        private readonly IProductQuery productQuery;

        public LatestArrivalsViewComponent(IProductQuery productQuery)
        {
            this.productQuery = productQuery;
        }

        public IViewComponentResult Invoke()
        {
            var products = productQuery.GetLatestArrivals();
            return View(products);
        }
    }
}
