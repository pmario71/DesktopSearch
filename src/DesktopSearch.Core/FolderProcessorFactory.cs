using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesktopSearch.Core.Configuration;
using DesktopSearch.Core.Processors;

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

            //TODO: use dependency injection to create processors, folder needs to be injected differently
            return (IFolderProcessor)Activator.CreateInstance(folderProcessorType, folder);
        }

    }

    public interface IFolderProcessor
    {
        Task Process();

        Task Process(IProgress<int> progress);
    }
}
