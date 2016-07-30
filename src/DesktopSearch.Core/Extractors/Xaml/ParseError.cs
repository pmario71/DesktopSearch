using System;

namespace CodeSearch.Extractors.Xaml
{
    public class ParseError
    {
        public string File { get; set; }
        public Exception Exception { get; set; }
    }
}