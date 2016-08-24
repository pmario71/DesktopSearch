using PowershellTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DesktopSearch.PS.Configuration
{
    public class AddDSFoldersToIndexCmdletTests
    {
        //TODO: Test cannot be setup in this way anymore, since ConfigAccess was moved to DesktopSearch.Core

        //[Fact]
        //public void xx()
        //{
        //    MemoryStream strm = new MemoryStreamEx();

        //    PSCmdLetTest.SetupAndRun<AddDSFoldersToIndexCmdlet>(
        //        builder =>
        //        {
        //            var tf = new TestFactory(strm);
        //            var ca = new ConfigAccess(tf);
        //            builder.SetupDependency(ca);
        //        },
        //        shell =>
        //    {
        //        shell.AddParameter("Path", "d:\\temp");

        //        var psObjects = shell.Invoke();
        //    });
        //}
    }
}
