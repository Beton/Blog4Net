using Blog4Net.Core.Infrastructure.IoC.Modules;
using Ninject;

namespace Blog4Net.Core.Infrastructure.IoC
{
    public static class NinjectBootstrapper
    {
        public static void LoadModules(IKernel kernel)
        {
            kernel.Load(new PersistenceModule());
        }        
    }
}