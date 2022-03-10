using OnlineStoreQuery.Contracts.Slide;
using ShopManagement.Infrastructure.EFCore;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStoreQuery.Query
{
    public class SlideQuery : ISlideQuery
    {
        private readonly ShopContext shopContext;

        public SlideQuery(ShopContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public List<SlideQueryModel> GetSlides()
        {
            return shopContext.Slides
                .Where(x => x.IsRemoved == false)
                .Select(x => new SlideQueryModel
                {
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    BtnText = x.BtnText,
                    Heading = x.Heading,
                    Link = x.Link,
                    Text = x.Text,
                    Title = x.Title
                }).ToList();
        }
    }
}
