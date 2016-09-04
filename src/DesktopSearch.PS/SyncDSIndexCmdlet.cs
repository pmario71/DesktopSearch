using DesktopSearch.Core;
using DesktopSearch.Core.Configuration;
using DesktopSearch.PS.Utils;
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

        [Import]
        public FolderProcessorFactory FolderProcessorFactory { get; set; }

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

            AsyncPump.Run(async () =>
            {
                var progress = new ProgressRecord(1, "Synching Index", "Folder");
                Action<int> progressCallback = p => 
                {
                    progress.PercentComplete = p;
                    WriteProgress(progress);
                };

                var aggregator = new DesktopSearch.Core.Utils.Async.AggregatingProgressReporter(progressCallback);

                foreach (var folder in foldersToSync)
                {
                    var processor = FolderProcessorFactory.GetProcessorByFolder(folder);

                    IProgress<int> pc = aggregator.CreateClient();

                    await processor.Process(folder, pc);
                }
            });
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
