using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using System.Collections.Generic;

namespace ShopManagementApplication.Contracts
{
    public interface IProductCategoryApplication
    {
        OperationResult Create(CreateProductCategory Command);
        OperationResult Edit(EditProductCategory Command);
        EditProductCategory GetDetails(long id);
        List<ProductCategoryViewModel> GetProductCategories();
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
    }
}
