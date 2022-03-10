using System;
using System.Collections.Generic;
using System.Linq;
using InventoryManagement.Application.Contract.Inventory;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl : IShopInventoryAcl
    {
        private readonly IInventoryApplication inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            this.inventoryApplication = inventoryApplication;
        }

        public bool ReduceFromInventory(List<OrderItem> items)
        {
            var command = items.Select(orderItem =>
                    new ReduceInventory(orderItem.ProductId, orderItem.Count, "خرید مشتری", orderItem.OrderId))
                .ToList();

            return inventoryApplication.Reduce(command).IsSuccedded;
        }
    }
}