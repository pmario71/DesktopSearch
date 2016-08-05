using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Code;
//using Lucene.Net.Documents;

namespace DesktopSearch.Core.Extractors.Xaml
{
    class XamlExtractor : IExtractor
    {
        private readonly XamlParser _xamlParser;

        public XamlExtractor(XamlParser xamlParser)
        {
            this._xamlParser = xamlParser;
        }

        public IEnumerable<TypeDescriptor> Extract(ParserContext context, FileInfo filePath)
        {
            // parser only extracts referenced types, but not the one implemented in the xaml
            var extractReferencedTypes = _xamlParser.ExtractReferencedTypes(filePath);

            //todo: map to TypeDescriptor
            return null;
        }
    }
}
