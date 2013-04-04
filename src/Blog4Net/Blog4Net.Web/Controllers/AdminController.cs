using System.Web.Mvc;
using Blog4Net.Web.Models;
using Blog4Net.Web.Services;

namespace Blog4Net.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAuthenticationService authenticationService;

        public AdminController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
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
    }
}
