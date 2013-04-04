using Blog4Net.Core.Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cache;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Blog4Net.Core.Infrastructure.IoC.Modules
{
    public class PersistenceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionFactory>()
                .ToMethod(
                    factory => Fluently.Configure()
                                       .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("blog4NetDB")))
                                       .Cache(c => c.UseQueryCache().ProviderClass<HashtableCacheProvider>())
                                       .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Post>())
                                       //.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                                       .BuildConfiguration()
                                       .BuildSessionFactory()
                ).InSingletonScope();

            Bind<ISession>()
                .ToMethod((ninject) => ninject.Kernel.Get<ISessionFactory>().OpenSession())
                .InRequestScope();
        }
    }
}