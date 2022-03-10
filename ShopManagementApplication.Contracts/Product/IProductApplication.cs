using _0_Framework.Application;
using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Product
{
    public interface IProductApplication
    {
        OperationResult Create(CreateProduct command);
        OperationResult Edit(EditProduct command);
        OperationResult ShortEdit(ShortEditProduct command);
        OperationResult IsStock(long id);
        OperationResult NotInStock(long id);
        EditProduct GetDetails(long id);
        ShortEditProduct GetShortDetails(long id);
        List<ProductViewModel> GetProducts();
        List<ProductViewModel> Search(ProductSearchModel searchModel);
    }
}
