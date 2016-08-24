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
    [Cmdlet(VerbsData.Sync, "DSIndex")]
    public class SyncDSIndexCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Folder(s) to sync.")]
        public string[] Folder { get; set; }


        #region Dependencies

        [Import]
        internal ConfigAccess ConfigAccess { set; get; }

        #endregion

        protected override void BeginProcessing()
        {
            this.Compose();
        }

        protected override void ProcessRecord()
        {
            
        }
    }
}
