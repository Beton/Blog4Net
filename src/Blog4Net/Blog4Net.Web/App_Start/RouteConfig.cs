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
                "Category",
                "Category/{category}",
                new {controller = "Blog", action = "Category"}
                );

            routes.MapRoute(
                "Action",
                "{action}",
                new { controller = "Blog", action = "Posts" }
            );
            routes.MapRoute(
                "Tag",
                "Tag/{tag}",
                new {controller = "Blog", action = "Tag"}
                );
        }
    }
}