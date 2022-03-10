using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStoreQuery.Contracts.Article;
using OnlineStoreQuery.Contracts.ArticleCategory;
using System.Collections.Generic;

namespace ServiceHost.Pages
{
    public class ArticleCategoryModel : PageModel
    {
        public ArticleCategoryQueryModel ArticleCategory;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        public List<ArticleQueryModel> LatestArticles;

        private readonly IArticleQuery articleQuery;
        private readonly IArticleCategoryQuery articleCategoryQuery;

        public ArticleCategoryModel(IArticleCategoryQuery articleCategoryQuery, IArticleQuery articleQuery)
        {
            this.articleQuery = articleQuery;
            this.articleCategoryQuery = articleCategoryQuery;
        }

        public void OnGet(string id)
        {
            LatestArticles = articleQuery.LatestArticles();
            ArticleCategory = articleCategoryQuery.GetArticleCategory(id);
            ArticleCategories = articleCategoryQuery.GetArticleCategories();
        }
    }
}
