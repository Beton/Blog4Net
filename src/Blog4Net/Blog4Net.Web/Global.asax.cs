using System.Web.Mvc;
using System.Web.Routing;
using Blog4Net.Core.DAL.Repositories;
using Blog4Net.Core.Infrastructure.IoC.Modules;
using Blog4Net.Web.App_Start;
using Ninject;
using Ninject.Web.Common;

namespace Blog4Net.Web
{    
    public class MvcApplication : NinjectHttpApplication
    {        
        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            
            kernel.Load(new RepositoryModule());
            kernel.Bind<IBlogRepository>().To<BlogRepository>();

            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            base.OnApplicationStarted();
        }
        
    }
}