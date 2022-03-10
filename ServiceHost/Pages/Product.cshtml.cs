using CommentManagement.Application.Contracts.Comment;
using CommnetManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStoreQuery.Contracts.Product;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        public ProductQueryModel Product;
        private readonly IProductQuery productQuery;
        private readonly ICommentApplication commentApplication;

        public ProductModel(IProductQuery productQuery, ICommentApplication commentApplication)
        {
            this.productQuery = productQuery;
            this.commentApplication = commentApplication;
        }

        public void OnGet(string id)
        {
            Product = productQuery.GetProductDetails(id);
        }

        public IActionResult OnPost(AddComment command, string productSlug)
        {
            command.Type = CommentType.Product;
            var result = commentApplication.Add(command);
            return RedirectToPage("/Product", new { Id = productSlug });
        }
    }
}
