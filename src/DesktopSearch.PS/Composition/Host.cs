using PowershellExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.ComponentModel.Composition;
using Microsoft.Extensions.Logging;
using DesktopSearch.Core.ElasticSearch;
using DesktopSearch.Core;

namespace DesktopSearch.PS.Composition
{
    internal class Host : HostBase
    {
        protected override CompositionContainer CreateAndInitializeContainer()
        {
            var conventions = new RegistrationBuilder();

            conventions.ForType<ClientFactory>()
                .SetCreationPolicy(CreationPolicy.Shared)
                .ExportProperties(p => p.Name.Contains("SearchClient"));

            conventions.ForType<Core.FolderProcessorFactory>().Export<Core.FolderProcessorFactory>();
            conventions.ForType<Core.Processors.CodeFolderProcessor>().Export<Core.Processors.CodeFolderProcessor>();
            conventions.ForType<Core.Processors.DocumentFolderProcessor>().Export<Core.Processors.DocumentFolderProcessor>();

            conventions.ForType<Core.Configuration.FileStreamFactory>().Export<Core.Configuration.IStreamFactory>();
            conventions.ForType<Core.Configuration.ConfigAccess>().Export<Core.Configuration.ConfigAccess>();

            var cat0 = new AssemblyCatalog(typeof(Host).Assembly);
            var cat1 = new AssemblyCatalog(typeof(Core.Configuration.FileStreamFactory).Assembly, conventions);
            var container = new CompositionContainer(new AggregateCatalog(cat0, cat1));

            var ca = new ContainerAccess(container);
            container.ComposeExportedValue<IContainer>(ca);

            return container;
        }
    }

    internal class HostTest
    {
        public void TryCreate(CompositionContainer container)
        {
            object obj = container.GetExportedValue<Core.FolderProcessorFactory>();
            obj = container.GetExportedValue<Core.Processors.CodeFolderProcessor>();
            obj = container.GetExportedValue<Core.Processors.DocumentFolderProcessor>();
        }
    }

    [Export(typeof(IContainer))]
    internal class ContainerAccess : IContainer
    {
        private CompositionContainer _container;

        public ContainerAccess(CompositionContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            var instance = _container.GetExports(serviceType, null, null).FirstOrDefault();

            if (instance == null)
            {
                throw new Exception($"Service not available: {serviceType.Name}");
            }
            return instance.Value;
        }
    }

    [Export(typeof(ILogger<>))]
    public class PowerShellLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Warning;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine(formatter(state, exception));
        }
    }
}
