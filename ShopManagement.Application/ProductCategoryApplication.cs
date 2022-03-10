using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagementApplication.Contracts;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IFileUploader fileUploader;
        private readonly IProductCategoryRepository productCategoryRepostory;

        public ProductCategoryApplication(IProductCategoryRepository productCategoryRepostory, IFileUploader fileUploader)
        {
            this.fileUploader = fileUploader;
            this.productCategoryRepostory = productCategoryRepostory;
            var operation = new OperationResult();
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var operation = new OperationResult();
            if (productCategoryRepostory.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();

            var picturePath = $"{command.Slug}";
            var pictureName = fileUploader.Upload(command.Picture, picturePath);

            var productCategory = new ProductCategory(command.Name, command.Description,
                pictureName, command.PictureAlt, command.PictureTitle, command.Keywords,
                command.MetaDescription, slug);

            productCategoryRepostory.Create(productCategory);
            productCategoryRepostory.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var operation = new OperationResult();
            var productCategory = productCategoryRepostory.Get(command.Id);

            if (productCategory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (productCategoryRepostory.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();

            var picturePath = $"{command.Slug}";
            var fileName = fileUploader.Upload(command.Picture, picturePath);

            productCategory.Edit(command.Name, command.Description, fileName,
                command.PictureAlt, command.PictureTitle, command.Keywords,
                command.MetaDescription, slug);

            productCategoryRepostory.SaveChanges();
            return operation.Succedded();
        }

        public EditProductCategory GetDetails(long id)
        {
            return productCategoryRepostory.GetDetails(id);
        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            return productCategoryRepostory.GetProductCategories();
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return productCategoryRepostory.Search(searchModel);
        }
    }
}
