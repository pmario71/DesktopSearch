using PowershellExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;

namespace DesktopSearch.PS.Composition
{
    class Host : HostBase
    {
        protected override CompositionContainer CreateAndInitializeContainer()
        {
            var cat = new AssemblyCatalog(typeof(Host).Assembly);
            return new CompositionContainer(cat);
        }
    }
}
