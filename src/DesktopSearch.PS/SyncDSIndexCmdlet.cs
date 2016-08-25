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
        private Settings _config;

        [Parameter(Mandatory = false, HelpMessage = "Folder(s) to sync.")]
        public string[] Folder { get; set; }


        #region Dependencies

        [Import]
        internal ConfigAccess ConfigAccess { set; get; }

        #endregion

        protected override void BeginProcessing()
        {
            this.Compose();

            _config = ConfigAccess.Get();
        }

        protected override void ProcessRecord()
        {
            List<Folder> foldersToSync = new List<Core.Configuration.Folder>();

            if (Folder != null)
            {
                MapPathsToConfiguredFolders(foldersToSync, this.Folder);
            }
            else
            {
                foldersToSync.AddRange(_config.FoldersToIndex.Folders);
            }

            foreach (var folder in foldersToSync)
            {

            }
        }

        private void MapPathsToConfiguredFolders(List<Folder> foldersToSync, IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                var foundFoder = _config.FoldersToIndex.Folders.FirstOrDefault(f => StringComparer.OrdinalIgnoreCase.Compare(f.Path, path) == 0);

                if (foundFoder == null)
                {
                    WriteWarning($"Ignored following folder, because it is not part of configuration: {path}");
                }
                else
                    foldersToSync.Add(foundFoder);
            }
        }
    }
}
