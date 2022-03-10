using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace InventoryManagement.Infrastructure.Configuration.Permissions
{
    public class InventoryPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Inventory", new List<PermissionDto>
                    {
                        new PermissionDto(InventoryPermissions.ListInventory, "فهرست موجودی"),
                        new PermissionDto(InventoryPermissions.SearchInventory, "جستجو موجودی"),
                        new PermissionDto(InventoryPermissions.CreateInventory, "ایجاد موجودی"),
                        new PermissionDto(InventoryPermissions.EditInventory, "ویرایش موجودی"),
                        new PermissionDto(InventoryPermissions.Increase, "افزایش موجودی"),
                        new PermissionDto(InventoryPermissions.Reduce, "كاهش موجودی"),
                        new PermissionDto(InventoryPermissions.OperationLog, "گزارش عملیات")
                    }
                }
            };
        }
    }
}