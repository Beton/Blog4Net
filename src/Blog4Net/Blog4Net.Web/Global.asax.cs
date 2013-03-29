using System;
using System.Web;
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
        private static IKernel kernel;
        private static readonly object Lock = new object();

        public new IKernel Kernel
        {
            get { return kernel; }
        }

        protected override IKernel CreateKernel()
        {
            lock (Lock)
            {
                kernel = new StandardKernel();

                kernel.Load(new RepositoryModule());
                kernel.Bind<IBlogRepository>().To<BlogRepository>();
            }
                                    
            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            base.OnApplicationStarted();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var httpError = ex as HttpException;

            if (httpError != null && httpError.GetHttpCode() == 404) return;

            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        }

        protected void OnApplicationEnded()
        {            
            kernel.Dispose();
        }
    }
}