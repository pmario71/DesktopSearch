using PowershellExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;

namespace DesktopSearch.PS.Composition
{
    class Host : HostBase
    {
        protected override CompositionContainer CreateAndInitializeContainer()
        {
            var conventions = new RegistrationBuilder();
            conventions.ForType<Core.Configuration.FileStreamFactory>().Export<Core.Configuration.IStreamFactory>();
            conventions.ForType<Core.Configuration.ConfigAccess>().Export<Core.Configuration.ConfigAccess>();

            var cat0 = new AssemblyCatalog(typeof(Host).Assembly);
            var cat1 = new AssemblyCatalog(typeof(Core.Configuration.FileStreamFactory).Assembly, conventions);
            return new CompositionContainer(new AggregateCatalog(cat0, cat1));
        }
    }
}
