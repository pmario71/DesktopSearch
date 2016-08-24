using DesktopSearch.Core.Configuration;
using PowershellExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.PS
{
    /// <summary>
    /// Returns configuration settings.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "DSSetting")]
    public class GetDSSettingCmdlet : PSCmdlet
    {
        #region Dependencies
        [Import]
        internal ConfigAccess ConfigAccess { set; get; }
        #endregion

        protected override void ProcessRecord()
        {
            this.Compose();

            WriteObject(ConfigAccess.Get());
        }
    }
}
