using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace ShopManagement.Configuration.Permissions
{
    public class ShopPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Product", new List<PermissionDto>
                    {
                        new PermissionDto(ShopPermissions.ListProducts, "لیست محصولات"),
                        new PermissionDto(ShopPermissions.SearchProducts, "جستجو محصولات"),
                        new PermissionDto(ShopPermissions.CreateProduct, "ایجاد محصول"),
                        new PermissionDto(ShopPermissions.EditProduct, "ویرایش محصول"),
                        new PermissionDto(ShopPermissions.ShortEditProduct, "ویرایش  کوتاه محصول"),
                    }
                },
                {
                    "ProductCategory", new List<PermissionDto>
                    {
                        new PermissionDto(ShopPermissions.SearchProductCategories, "جستجو در دسته بندی محصولات"),
                        new PermissionDto(ShopPermissions.ListProductCategories, "لیست دسته بندی محصولات"),
                        new PermissionDto(ShopPermissions.CreateProductCategory, "ایجاد دسته بندی محصولات"),
                        new PermissionDto(ShopPermissions.EditProductCategory, "ویرایش دسته محصولات"),
                    }
                }
            };
        }
    }
}