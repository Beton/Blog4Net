namespace Blog4Net.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Blog4Net.Core.DAL.Repositories;
    using Blog4Net.Core.Domain;
    using Blog4Net.Core.Infrastructure.IoC;
    using Blog4Net.Web.App_Start;
    using Blog4Net.Web.Infrastructure.ModelBinders;
    using Blog4Net.Web.Services;

    using Ninject;
    using Ninject.Web.Common;

    using StackExchange.Profiling;

    public class MvcApplication : NinjectHttpApplication
    {            
        protected override IKernel CreateKernel()
        {            
            var kernel = new StandardKernel();

            NinjectBootstrapper.LoadModules(kernel);

            kernel.Bind<IBlogRepository>().To<BlogRepository>();
            kernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>();
                                                                                    
            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            base.OnApplicationStarted();

            ModelBinders.Binders.Add(typeof(Post), new PostModelBinder(Kernel));
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal) MiniProfiler.Start();
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var httpError = ex as HttpException;

            if (httpError != null && httpError.GetHttpCode() == 404) return;

            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
        }        
    }
}