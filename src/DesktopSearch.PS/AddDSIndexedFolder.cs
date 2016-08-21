using DesktopSearch.PS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.PS
{
    [Cmdlet(VerbsCommon.Add,"DSIndexedFolder", DefaultParameterSetName = "All")]
    public class AddDSIndexedFolder : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Paths of folders to add.")]
        [ValidatePath()]
        public string[] Path { get; set; }

        [Parameter(Mandatory=false, HelpMessage ="List of file extensions to exclude from indexing [e.g. @('txt', 'docx') ]", ParameterSetName ="Exclude")]
        public string[] ExcludeExtensions { get; set; }

        [Parameter(Mandatory = false, 
                   HelpMessage = "List of file extensions to include in indexing [e.g. @('txt', 'docx') ]. All files with different extension will be ignored.",
                   ParameterSetName = "Include")]
        public string[] IncludeExtensions { get; set; }

        protected override void BeginProcessing()
        {
            if (ParameterSetName == "Exclude")
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessRecord()
        {
            
        }

    }
}
