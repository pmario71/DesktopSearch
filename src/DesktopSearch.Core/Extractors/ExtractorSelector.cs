using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSearch.Extractors.Roslyn;
using CodeSearch.Extractors.Xaml;

namespace CodeSearch.Extractors
{
    class ExtractorSelector
    {
        static readonly Dictionary<string,IExtractor> Map = new Dictionary<string, IExtractor>(StringComparer.OrdinalIgnoreCase);

        static ExtractorSelector()
        {
            Map.Add("cs", new Roslyn.RoslynExtractor());
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
