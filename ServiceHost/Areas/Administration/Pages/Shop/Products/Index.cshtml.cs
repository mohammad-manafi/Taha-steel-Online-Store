using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using ShopManagement.Configuration.Permissions;
using ShopManagementApplication.Contracts;

namespace ServiceHost.Areas.Administration.Pages.Shop.Products
{
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }
        public ProductSearchModel SearchModel;
        public List<ProductViewModel> Products;
        public SelectList ProductCategories;

        private readonly IProductApplication productApplication;
        private readonly IProductCategoryApplication productCategoryApplication;

        public IndexModel(IProductApplication productApplication,
            IProductCategoryApplication productCategoryApplication)
        {
            this.productApplication = productApplication;
            this.productCategoryApplication = productCategoryApplication;
        }

        [NeedsPermission(ShopPermissions.ListProducts)]
        public void OnGet(ProductSearchModel searchModel)
        {
            ProductCategories = new SelectList(productCategoryApplication.GetProductCategories(), "Id", "Name");
            Products = productApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateProduct
            {
                Categories = productCategoryApplication.GetProductCategories()
            };
            return Partial("./Create", command);
        }

        [NeedsPermission(ShopPermissions.CreateProduct)]
        public JsonResult OnPostCreate(CreateProduct command)
        {
            var result = productApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var product = productApplication.GetDetails(id);
            product.Categories = productCategoryApplication.GetProductCategories();
            return Partial("Edit", product);
        }

        [NeedsPermission(ShopPermissions.EditProduct)]
        public JsonResult OnPostEdit(EditProduct command)
        {
            var result = productApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetShortEdit(long id)
        {
            var product = productApplication.GetShortDetails(id);
            product.Categories = productCategoryApplication.GetProductCategories();
            return Partial("ShortEdit", product);
        }

        [NeedsPermission(ShopPermissions.ShortEditProduct)]
        public JsonResult OnPostShortEdit(ShortEditProduct command)
        {
            var result = productApplication.ShortEdit(command);
            return new JsonResult(result);
        }

    }
}