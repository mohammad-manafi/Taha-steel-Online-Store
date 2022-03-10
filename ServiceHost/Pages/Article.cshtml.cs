using CommentManagement.Application.Contracts.Comment;
using CommnetManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStoreQuery.Contracts.Article;
using OnlineStoreQuery.Contracts.ArticleCategory;
using System.Collections.Generic;

namespace ServiceHost.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleQueryModel Article;
        public List<ArticleQueryModel> LatestArticles;
        public List<ArticleCategoryQueryModel> ArticleCategories;
        private readonly IArticleQuery articleQuery;
        private readonly IArticleCategoryQuery articleCategoryQuery;
        private readonly ICommentApplication commentApplication;

        public ArticleModel(IArticleQuery articleQuery, IArticleCategoryQuery articleCategoryQuery, ICommentApplication commentApplication)
        {
            this.articleQuery = articleQuery;
            this.commentApplication = commentApplication;
            this.articleCategoryQuery = articleCategoryQuery;
        }

        public void OnGet(string id)
        {
            Article = articleQuery.GetArticleDetails(id);
            LatestArticles = articleQuery.LatestArticles();
            ArticleCategories = articleCategoryQuery.GetArticleCategories();
        }

        public IActionResult OnPost(AddComment command, string articleSlug)
        {
            command.Type = CommentType.Article;
            var result = commentApplication.Add(command);
            return RedirectToPage("/Article", new { Id = articleSlug });
        }
    }
}
