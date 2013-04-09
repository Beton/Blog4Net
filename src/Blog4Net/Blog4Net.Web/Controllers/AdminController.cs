using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Core.Domain;
using Blog4Net.Web.Models;
using Blog4Net.Web.Models.ViewModels;
using Blog4Net.Web.Services;
using Blog4Net.Web.Utilities;
using Newtonsoft.Json;

namespace Blog4Net.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IBlogRepository blogRepository;

        public AdminController(IAuthenticationService authenticationService, IBlogRepository blogRepository)
        {
            this.authenticationService = authenticationService;
            this.blogRepository = blogRepository;
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (authenticationService.IsLogged)
                return RedirectToAction("Manage");         
            
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid && authenticationService.LogOn(loginModel.Username, loginModel.Password))
            {
                return RedirectToAction("Manage");              
            }

            ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            authenticationService.LogOff();

            return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public ActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public ContentResult Posts(GridViewModel gridParams)
        {
            var posts = blogRepository.Posts(gridParams.Page - 1, gridParams.Rows, gridParams.Sidx, gridParams.sord == "asc");

            var totalPosts = blogRepository.TotalPosts(false);

            var @object = new {
                    page = gridParams.Page, 
                    records = posts.Count, 
                    rows = posts, 
                    total = Math.Ceiling(Convert.ToDouble(totalPosts)/gridParams.Rows)
                };

            return Content(JsonConvert.SerializeObject(@object, new CustomDateTimeConverter()), "application/json");
        }        

        [HttpPost]
        [ValidateInput(false)]
        public ContentResult AddPost(Post post)
        {
            string json;

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                var postId = blogRepository.AddPost(post);

                json = JsonConvert.SerializeObject(new
                    {
                        id = postId,
                        success = true,
                        message = "Post added successfully."
                    });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the post."
                });
            }

            return Content(json, "application/json");
        }

        [HttpGet]
        public ContentResult GetCategoriesHtml()
        {
            var categories = blogRepository.Categories().OrderBy(c => c.Name);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<select>");

            foreach (var category in categories)
            {
                stringBuilder.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>", category.Id, category.Name));
            }

            stringBuilder.AppendLine(@"<select>");

            return Content(stringBuilder.ToString(), "text/html");
        }

        [HttpGet]
        public ContentResult GetTagsHtml()
        {
            var tags = blogRepository.Tags().OrderBy(t => t.Name);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(@"<select multiple=""multiple"">");

            foreach (var tag in tags)
            {
                stringBuilder.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>", tag.Id, tag.Name));
            }

            stringBuilder.AppendLine("<select>");
            
            return Content(stringBuilder.ToString(), "text/html");
        }
    }
}
