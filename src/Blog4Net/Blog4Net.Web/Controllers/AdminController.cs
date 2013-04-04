using System;
using System.Web.Mvc;
using Blog4Net.Core.DAL.Repositories;
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
    }
}
