using Microsoft.AspNetCore.Mvc;
using OnlineStoreQuery.Contracts.Article;

namespace ServiceHost.ViewComponents
{
    public class LatestArticlesViewComponent : ViewComponent
    {
        private readonly IArticleQuery articleQuery;

        public LatestArticlesViewComponent(IArticleQuery articleQuery)
        {
            this.articleQuery = articleQuery;
        }

        public IViewComponentResult Invoke()
        {
            var articles = articleQuery.LatestArticles();
            return View(articles);
        }
    }
}
