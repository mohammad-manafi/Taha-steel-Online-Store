using Microsoft.AspNetCore.Mvc;
using OnlineStoreQuery.Contracts.ProductCategory;

namespace ServiceHost.ViewComponents
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery productCategoryQuery;

        public ProductCategoryViewComponent(IProductCategoryQuery productCategoryQuery)
        {
            this.productCategoryQuery = productCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            var productCategories = productCategoryQuery.GetProductCategories();
            return View(productCategories);
        }
    }
}
