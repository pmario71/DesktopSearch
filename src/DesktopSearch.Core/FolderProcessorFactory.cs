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

        public IFolderProcessor GetProcessorByFolderType(string indexingTypeName)
        {
            Type folderProcessorType;
            if (!_map.TryGetValue(indexingTypeName, out folderProcessorType))
            {
                throw new ArgumentOutOfRangeException("indexingTypeName", $"'{indexingTypeName}' is unknown!");
            }

            return (IFolderProcessor)Activator.CreateInstance(folderProcessorType);
        }

    }

    internal class DocumentFolderProcessor : IFolderProcessor
    {
    }

    internal class CodeFolderProcessor : IFolderProcessor
    {
    }

    public interface IFolderProcessor
    {
    }
}
