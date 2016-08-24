using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopSearch.Core
{
    public class Container
    {
        private IServiceProvider serviceCollection;

        private Container(IServiceProvider serviceProvider)
        {
            this.serviceCollection = serviceProvider;
        }

        public static Container Initialize(Action<IServiceCollection> initializer = null)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            if (initializer != null)
            {
                initializer(serviceCollection);
            }

            var container = new Container(serviceCollection.BuildServiceProvider());
            return container;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Configuration.IStreamFactory, Configuration.FileStreamFactory>();
        }

    }
}
