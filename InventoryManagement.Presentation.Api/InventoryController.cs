using System.Collections.Generic;
using InventoryManagement.Application.Contract.Inventory;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreQuery.Contracts.Inventory;

namespace InventoryManagement.Presentation.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryQuery inventoryQuery;
        private readonly IInventoryApplication inventoryApplication;

        public InventoryController(IInventoryQuery inventoryQuery, IInventoryApplication inventoryApplication)
        {
            this.inventoryQuery = inventoryQuery;
            this.inventoryApplication = inventoryApplication;
        }   

        [HttpGet("{id}")]
        public List<InventoryOperationViewModel> GetOperationsBy(long id)
        {
            return inventoryApplication.GetOperationLog(id);
        }

        [HttpPost]
        public StockStatus CheckStock(IsInStock command)
        {
            return inventoryQuery.CheckStock(command);
        }
    }
}