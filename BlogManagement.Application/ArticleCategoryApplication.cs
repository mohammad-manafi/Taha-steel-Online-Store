using _0_Framework.Application;
using BlogManagement.Application.Contracts.ArticleCategory;
using BlogManagement.Domain.ArticleCategoryAgg;
using System.Collections.Generic;

namespace BlogManagement.Application
{
    public class ArticleCategoryApplication : IArticleCategoryApplication
    {
        private readonly IFileUploader fileUploader;
        private readonly IArticleCategoryRepository articleCategoryRepository;

        public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository, IFileUploader fileUploader)
        {
            this.fileUploader = fileUploader;
            this.articleCategoryRepository = articleCategoryRepository;
        }

        public OperationResult Create(CreateArticleCategory command)
        {
            var operation = new OperationResult();
            if (articleCategoryRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var pictureName = fileUploader.Upload(command.Picture, slug);
            var articleCategory = new ArticleCategory(command.Name, pictureName, command.PictureAlt, command.PictureTitle
                , command.Description, command.ShowOrder, slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress);

            articleCategoryRepository.Create(articleCategory);
            articleCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditArticleCategory command)
        {
            var operation = new OperationResult();
            var articleCategory = articleCategoryRepository.Get(command.Id);

            if (articleCategory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (articleCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var pictureName = fileUploader.Upload(command.Picture, slug);
            articleCategory.Edit(command.Name, pictureName, command.PictureAlt, command.PictureTitle,
                command.Description, command.ShowOrder, slug, command.Keywords, command.MetaDescription,
                command.CanonicalAddress);

            articleCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public List<ArticleCategoryViewModel> GetArticleCategories()
        {
            return articleCategoryRepository.GetArticleCategories();
        }

        public EditArticleCategory GetDetails(long id)
        {
            return articleCategoryRepository.GetDetails(id);
        }

        public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
        {
            return articleCategoryRepository.Search(searchModel);
        }
    }
}
