using DesktopSearch.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Processors
{
    internal class DocumentFolderProcessor : IFolderProcessor
    {
        private Folder _folder;

        public DocumentFolderProcessor(Folder folder)
        {
            _folder = folder;
        }

        public Task Process()
        {
            return Process(null);
        }

        public Task Process(IProgress<int> progress)
        {
            return null;
        }
    }
}
