using Microsoft.AspNetCore.Mvc;
using OnlineStoreQuery.Contracts.ProductCategory;

namespace ServiceHost.ViewComponents
{
    public class ProductCategoryWithProductViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery productCategoryQuery;

        public ProductCategoryWithProductViewComponent(IProductCategoryQuery productCategoryQuery)
        {
            this.productCategoryQuery = productCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            var categories = productCategoryQuery.GetProductCategoriesWithProducts();
            return View(categories);
        }
    }
}
