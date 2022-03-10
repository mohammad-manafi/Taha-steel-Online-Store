using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using InventoryManagement.Infrastructure.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.Inventory
{
    [Authorize(Roles = Roles.Administrator)]
    public class IndexModel : PageModel
    {
        [TempData] public string Message { get; set; }
        public InventorySearchModel SearchModel;
        public List<InventoryViewModel> Inventory;
        public SelectList Products;

        private readonly IProductApplication productApplication;
        private readonly IInventoryApplication inventoryApplication;

        public IndexModel(IProductApplication productApplication, IInventoryApplication inventoryApplication)
        {
            this.productApplication = productApplication;
            this.inventoryApplication = inventoryApplication;
        }

        [NeedsPermission(InventoryPermissions.ListInventory)]
        public void OnGet(InventorySearchModel searchModel)
        {
            Products = new SelectList(productApplication.GetProducts(), "Id", "Name");
            Inventory = inventoryApplication.Search(searchModel);
        }

        [NeedsPermission(InventoryPermissions.CreateInventory)]
        public IActionResult OnGetCreate()
        {
            var command = new CreateInventory
            {
                Products = productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        [NeedsPermission(InventoryPermissions.CreateInventory)]
        public JsonResult OnPostCreate(CreateInventory command)
        {
            var result = inventoryApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var inventory = inventoryApplication.GetDetails(id);
            inventory.Products = productApplication.GetProducts();
            return Partial("Edit", inventory);
        }

        [NeedsPermission(InventoryPermissions.EditInventory)]
        public JsonResult OnPostEdit(EditInventory command)
        {
            var result = inventoryApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetIncrease(long id)
        {
            var command = new IncreaseInventory()
            {
                InventoryId = id
            };
            return Partial("Increase", command);
        }

        [NeedsPermission(InventoryPermissions.Increase)]
        public JsonResult OnPostIncrease(IncreaseInventory command)
        {
            var result = inventoryApplication.Increase(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetReduce(long id)
        {
            var command = new ReduceInventory()
            {
                InventoryId = id
            };
            return Partial("Reduce", command);
        }

        [NeedsPermission(InventoryPermissions.Reduce)]
        public JsonResult OnPostReduce(ReduceInventory command)
        {
            var result = inventoryApplication.Reduce(command);
            return new JsonResult(result);
        }

        [NeedsPermission(InventoryPermissions.OperationLog)]
        public IActionResult OnGetLog(long id)
        {
            var log = inventoryApplication.GetOperationLog(id);
            return Partial("OperationLog", log);
        }
    }
}