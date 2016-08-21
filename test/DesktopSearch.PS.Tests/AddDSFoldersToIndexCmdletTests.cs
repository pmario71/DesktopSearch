using PowershellTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DesktopSearch.PS.Configuration
{
    public class AddDSFoldersToIndexCmdletTests
    {

        [Fact]
        public void xx()
        {
            PSCmdLetTest.SetupAndRun<AddDSFoldersToIndexCmdlet>(shell =>
            {
                shell.AddParameter("Path", "d:\\temp");

                var psObjects = shell.Invoke();


            });
        }

    }
}
