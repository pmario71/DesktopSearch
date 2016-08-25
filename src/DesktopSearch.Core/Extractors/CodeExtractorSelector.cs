using DesktopSearch.Core.Extractors.Roslyn;
using DesktopSearch.Core.Extractors.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Extractors
{
    class CodeExtractorSelector
    {
        static readonly Dictionary<string,IExtractor> Map = new Dictionary<string, IExtractor>(StringComparer.OrdinalIgnoreCase);

        static CodeExtractorSelector()
        {
            Map.Add("cs", new RoslynExtractor());
            Map.Add("xaml", new XamlExtractor(new Xaml.XamlParser(new XamlParserConfiguration())));
        }

        public static IExtractor GetExtractorFromExtension(string extension)
        {
            IExtractor extractor;
            if (Map.TryGetValue(extension, out extractor))
            {
                return extractor;
            }
            return null;
        }
    }
}
