using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.ProductCategory;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using ShopManagement.Configuration.Permissions;
using ShopManagementApplication.Contracts;

namespace ServiceHost.Areas.Administration.Pages.Shop.ProductCategories
{
    //[Authorize(Roles = "1, 3")]
    public class IndexModel : PageModel
    {
        public ProductCategorySearchModel SearchModel;
        public List<ProductCategoryViewModel> ProductCategories;

        private readonly IProductCategoryApplication productCategoryApplication;

        public IndexModel(IProductCategoryApplication productCategoryApplication)
        {
            this.productCategoryApplication = productCategoryApplication;
        }

        [NeedsPermission(ShopPermissions.ListProductCategories)]
        public void OnGet(ProductCategorySearchModel searchModel)
        {
            ProductCategories = productCategoryApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateProductCategory());
        }

        [NeedsPermission(ShopPermissions.CreateProductCategory)]
        public JsonResult OnPostCreate(CreateProductCategory command)
        {
            var result = productCategoryApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var productCategory = productCategoryApplication.GetDetails(id);
            return Partial("Edit", productCategory);
        }

        [NeedsPermission(ShopPermissions.EditProductCategory)]
        public JsonResult OnPostEdit(EditProductCategory command)
        {
            if (ModelState.IsValid)
            {
            }

            var result = productCategoryApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}