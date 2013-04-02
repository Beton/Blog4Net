using System.Web.Mvc;
using System.Web.Security;
using Blog4Net.Web.Models;

namespace Blog4Net.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        [HttpGet, AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (FormsAuthentication.Authenticate(loginModel.Username, loginModel.Password))
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Username, false);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Manage", "Admin");
                }

                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Admin");
        }

        [HttpGet]
        public ActionResult Manage()
        {
            return Content("TODO");
        }
    }
}
