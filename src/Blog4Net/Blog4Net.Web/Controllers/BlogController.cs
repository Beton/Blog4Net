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

            ViewBag.Title = "Latests Posts";

            return View("List", listViewModel);
        }
    }
}
