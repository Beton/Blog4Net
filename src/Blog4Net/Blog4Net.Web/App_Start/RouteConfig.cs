using System.Web.Mvc;
using System.Web.Routing;

namespace Blog4Net.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Action",
                "{action}",
                new { controller = "Blog", action = "Posts" }
            );
        }
    }
}