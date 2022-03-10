using Microsoft.AspNetCore.Mvc;
using OnlineStoreQuery;
using OnlineStoreQuery.Contracts.ArticleCategory;
using OnlineStoreQuery.Contracts.ProductCategory;

namespace ServiceHost.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery productCategoryQuery;
        private readonly IArticleCategoryQuery articleCategoryQuery;
        public MenuViewComponent(IProductCategoryQuery productCategoryQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            this.articleCategoryQuery = articleCategoryQuery;
            this.productCategoryQuery = productCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            var result = new MenuModel
            {
                ArticleCategories = articleCategoryQuery.GetArticleCategories(),
                ProductCategories = productCategoryQuery.GetProductCategories()
            };
            return View(result);
        }
    }
}
