using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Configuration
{
    public class Settings
    {
        public FoldersToIndex FoldersToIndex { get; set; }
    }

    public class FoldersToIndex
    {
        public Folder[] Folders { get; set; }
    }

    public class Folder
    {
        public string Path { get; set; }

        /// <summary>
        /// Defines which type of indexing to apply to the folder.
        /// </summary>
        public string IndexingType { get; set; }
    }

    public class IndexingType
    {
        public string Name { get; set; }

        public string[] ExcludedExtensions { get; set; }

        public string[] IncludedExtensions { get; set; }
    }
}
