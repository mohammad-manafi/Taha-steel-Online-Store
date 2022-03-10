using OnlineStoreQuery.Contracts.ArticleCategory;
using OnlineStoreQuery.Contracts.ProductCategory;
using System.Collections.Generic;

namespace OnlineStoreQuery
{
    public class MenuModel
    {
        public List<ArticleCategoryQueryModel> ArticleCategories { get; set; }
        public List<ProductCategoryQueryModel> ProductCategories { get; set; }
    }
}
