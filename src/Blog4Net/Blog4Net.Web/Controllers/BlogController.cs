using System;
using System.Web;
using System.Web.Mvc;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Web.Models;

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

            ViewBag.Title = "Black sheep  | Beton | Latests posts";

            return View("List", listViewModel);
        }

        public ViewResult Category(string category, int pageNumber = 1)
        {
            var viewModel = new ListViewModel(blogRepository, category, pageNumber);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found :(");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""", viewModel.Category.Name);

            return View("List", viewModel);
        }
    }
}
