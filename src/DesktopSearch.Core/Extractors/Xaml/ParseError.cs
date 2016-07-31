using System;

namespace DesktopSearch.Core.Extractors.Xaml
{
    public class ParseError
    {
        public string File { get; set; }
        public Exception Exception { get; set; }
    }
}