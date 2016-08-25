using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesktopSearch.Core.Configuration;

namespace DesktopSearch.Core
{
    public class FolderProcessorFactory
    {
        private Dictionary<string, Type> _map = new Dictionary<string, Type>()
        {
            { "Code"      , typeof(CodeFolderProcessor) },
            { "Documents" , typeof(DocumentFolderProcessor) },
        };

        public IFolderProcessor GetProcessorByFolder(Folder folder)
        {
            if (folder == null)
                throw new ArgumentNullException("folder");

            Type folderProcessorType;
            if (!_map.TryGetValue(folder.IndexingType, out folderProcessorType))
            {
                throw new ArgumentOutOfRangeException("indexingTypeName", $"'{folder.IndexingType}' is unknown!");
            }

            return (IFolderProcessor)Activator.CreateInstance(folderProcessorType);
        }

    }

    internal class DocumentFolderProcessor : IFolderProcessor
    {
        public Task Process()
        {
            return Process(null);
        }

        public Task Process(IProgress<int> progress)
        {
            return null;
        }
    }

    internal class CodeFolderProcessor : IFolderProcessor
    {
        public Task Process()
        {
            return Process(null);
        }

        public Task Process(IProgress<int> progress)
        {
            return null;
        }
    }

    public interface IFolderProcessor
    {
        Task Process();

        Task Process(IProgress<int> progress);
    }
}
