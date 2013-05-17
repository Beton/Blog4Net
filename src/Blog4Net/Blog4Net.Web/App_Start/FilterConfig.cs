using System.Web.Mvc;

namespace Blog4Net.Web.App_Start
{
    using StackExchange.Profiling.MVCHelpers;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ProfilingActionFilter());
        }
    }
}