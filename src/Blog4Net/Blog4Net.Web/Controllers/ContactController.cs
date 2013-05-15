namespace Blog4Net.Web.Controllers
{
    using System.Web.Mvc;

    public class ContactController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ContactMe")]
        public ActionResult SaveContactData()
        {
            return null;
        }
    }
}