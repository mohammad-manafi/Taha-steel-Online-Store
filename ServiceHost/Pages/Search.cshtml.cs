using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStoreQuery.Contracts.Product;
using System.Collections.Generic;

namespace ServiceHost.Pages
{
    public class SearchModel : PageModel
    {
        public string Value;
        public List<ProductQueryModel> Products;
        private readonly IProductQuery productQuery;

        public SearchModel(IProductQuery productQuery)
        {
            this.productQuery = productQuery;
        }

        public void OnGet(string value)
        {
            Value = value;
            Products = productQuery.Search(value);
        }
    }
}
