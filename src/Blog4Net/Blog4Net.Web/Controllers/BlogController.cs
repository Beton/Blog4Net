using System;
using System.Web;
using System.Web.Mvc;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Web.Models;
using Blog4Net.Web.Models.ViewModels;

namespace Blog4Net.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public ViewResult Posts(int pageNumber = 1)
        {
            var listViewModel = new ListViewModel(blogRepository, pageNumber);                

            ViewBag.Title = "Latests posts";

            return View("List", listViewModel);
        }

        public ViewResult Category(string category, int pageNumber = 1)
        {
            var viewModel = new ListViewModel(blogRepository, category, "Category", pageNumber);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found :(");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""", viewModel.Category.Name);

            return View("List", viewModel);
        }

        public ViewResult Tag(string tag, int pageNumber = 1)
        {
            var viewModel = new ListViewModel(blogRepository, tag, "Tag", pageNumber);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found :(");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""", viewModel.Tag.Name);

            return View("List", viewModel);           
        }

        public ViewResult Search(string searchCritera, int pageNumber = 1)
        {
            ViewBag.Title = String.Format(@"Lists of posts found for search text ""{0}""", searchCritera);

            var viewModel = new ListViewModel(blogRepository, searchCritera, "Search", pageNumber);
            
            return View("List", viewModel);
        }

        public ViewResult Post(int year, int month, string title)
        {
            var post = blogRepository.Post(year, month, title);

            if (post == null) 
                throw new HttpException(404, string.Format(@"Post '{0}' not found.", title));

            if (!post.Published && !User.Identity.IsAuthenticated)
                throw new HttpException(401, string.Format(@"Post '{0}' is not published.", title));

            return View(post);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var widgetViewModel = new WidgetViewModel(blogRepository);

            return PartialView("_Sidebars", widgetViewModel);
        }
    }
}
