using Microsoft.AspNetCore.Mvc;
using OnlineStoreQuery.Contracts.Slide;

namespace ServiceHost.ViewComponents
{
    public class SlideViewComponent : ViewComponent
    {
        private readonly ISlideQuery slideQuery;

        public SlideViewComponent(ISlideQuery slideQuery)
        {
            this.slideQuery = slideQuery;
        }

        public IViewComponentResult Invoke()
        {
            var slides = slideQuery.GetSlides();
            return View(slides);
        }
    }
}
