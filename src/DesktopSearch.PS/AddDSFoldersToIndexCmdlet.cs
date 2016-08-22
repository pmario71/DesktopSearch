using DesktopSearch.Core.FileSystem;
using DesktopSearch.PS.Configuration;
using DesktopSearch.PS.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    [Cmdlet(VerbsCommon.Add, "DSFoldersToIndex", DefaultParameterSetName = "All")]
    public class AddDSFoldersToIndexCmdlet : PSCmdlet
    {
        private Func<string, bool> _filterPredicate;
        private Settings _settings;
        private List<Folder> _folders;

        [Parameter(Mandatory = true, HelpMessage = "Paths of folders to add.")]
        [ValidatePath()]
        public string[] Path { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "List of file extensions to exclude from indexing [e.g. @('txt', 'docx') ]", ParameterSetName = "Exclude")]
        public string[] ExcludeExtensions { get; set; }

        [Parameter(Mandatory = false,
                   HelpMessage = "List of file extensions to include in indexing [e.g. @('txt', 'docx') ]. All files with different extension will be ignored.",
                   ParameterSetName = "Include")]
        public string[] IncludeExtensions { get; set; }


        #region Dependencies
        [Import]
        internal ConfigAccess ConfigAccess { set; get; }
        #endregion

        protected override void BeginProcessing()
        {
            this.Compose();

            if (ParameterSetName == "Exclude")
            {
                var filter = new ExcludeFileByExtensionFilter(this.ExcludeExtensions);
                _filterPredicate = t => filter.FilterByExtension(t);

            }
            else if (ParameterSetName == "Include")
            {
                var filter = new IncludeFileByExtensionFilter(this.IncludeExtensions);
                _filterPredicate = t => filter.FilterByExtension(t);
            }

            try
            {
                _settings = ConfigAccess.Get();
                _folders = new List<Folder>(_settings.FoldersToIndex.Folders ?? new Configuration.Folder[0]);
            }
            catch(Exception ex)
            {
                WriteWarning($"Recovering from error loading configuration: {ex.Message}");
                _settings = new Settings()
                {
                    FoldersToIndex = new FoldersToIndex()
                };
                _folders = new List<Folder>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessRecord()
        {
            foreach (var p in Path)
            {
                var folder = new Folder
                {
                    Path = p,
                    ExcludedExtensions = this.ExcludeExtensions,
                    IncludedExtensions = this.IncludeExtensions
                };
                _folders.Add(folder);
                WriteVerbose($"Added folder: {folder.Path}\r\n   Exc [{folder.ExcludedExtensions}]\r\n    Inc [{folder.IncludedExtensions}] )");
            }
        }

        protected override void EndProcessing()
        {
            _settings.FoldersToIndex.Folders = _folders.ToArray();
            ConfigAccess.SaveChanges(_settings);
        }
    }
}
