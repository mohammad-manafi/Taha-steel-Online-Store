using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IFileUploader fileUploader;
        private readonly IProductRepository productRepository;
        private readonly IProductCategoryRepository productCategoryRepository;

        public ProductApplication(IProductRepository productRepository, IFileUploader fileUploader, IProductCategoryRepository productCategoryRepository)
        {
            this.fileUploader = fileUploader;
            this.productRepository = productRepository;
            this.productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create(CreateProduct command)
        {
            var operation = new OperationResult();
            if (productRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var categorySlug = productCategoryRepository.GetSlugById(command.CategoryId);
            var path = $"{categorySlug}//{slug}";
            var picturePath = fileUploader.Upload(command.Picture, path);
            var product = new Product(command.Name, command.Code,command.UnitPrice,
                command.ShortDescription, command.Description, picturePath,
                command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                command.Keywords, command.MetaDescription);
            productRepository.Create(product);
            productRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditProduct command)
        {
            var operation = new OperationResult();
            var product = productRepository.GetProductWithCategory(command.Id);
            if (product == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var path = $"{product.Category.Slug}/{slug}";

            var picturePath = fileUploader.Upload(command.Picture, path);
            product.Edit(command.Name, command.Code,command.UnitPrice,
                command.ShortDescription, command.Description, picturePath,
                command.PictureAlt, command.PictureTitle, command.CategoryId, slug,
                command.Keywords, command.MetaDescription);

            productRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditProduct GetDetails(long id)
        {
            return productRepository.GetDetails(id);
        }

        public OperationResult ShortEdit(ShortEditProduct command)
        {
            var operation = new OperationResult();
            var product = productRepository.GetProductWithCategory(command.Id);
            if (product == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            
            product.ShortEdit(command.Name, command.Code, command.UnitPrice,command.CategoryId);

            productRepository.SaveChanges();
            return operation.Succedded();
        }

        public ShortEditProduct GetShortDetails(long id)
        {
            return productRepository.GetShortDetails(id);
        }

        public List<ProductViewModel> GetProducts()
        {
            return productRepository.GetProducts();
        }

        public OperationResult IsStock(long id)
        {
            var operation = new OperationResult();
            var product = productRepository.GetProductWithCategory(id);
            if (product == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);


            product.InStock();
            productRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult NotInStock(long id)
        {
            var operation = new OperationResult();
            var product = productRepository.GetProductWithCategory(id);
            if (product == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);


            product.NotInStock();
            productRepository.SaveChanges();
            return operation.Succedded();
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            return productRepository.Search(searchModel);
        }
    }
}
