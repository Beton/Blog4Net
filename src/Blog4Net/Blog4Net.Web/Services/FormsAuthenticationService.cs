using System.Web;
using System.Web.Security;

namespace Blog4Net.Web.Services
{
    public class FormsAuthenticationService : IAuthenticationService
    {                   
        public bool IsLogged
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }
        
        public bool LogOn(string username, string password)
        {
            var authenticated = FormsAuthentication.Authenticate(username, password);

            if (authenticated)
                FormsAuthentication.SetAuthCookie(username, false);

            return authenticated;
        }

        public void LogOff()
        {
            FormsAuthentication.SignOut();
        }
    }
}