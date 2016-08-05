using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Extractors.Roslyn
{
    class RoslynExtractor : IExtractor
    {
        private RoslynParser _roslynParser;

        public RoslynExtractor()
        {
            _roslynParser = new RoslynParser();
        }
        public IEnumerable<TypeDescriptor> Extract(ParserContext context, FileInfo filePath)
        {
            throw new NotImplementedException();
        }
    }
}
