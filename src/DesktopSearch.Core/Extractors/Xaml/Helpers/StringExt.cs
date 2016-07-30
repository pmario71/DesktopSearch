using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class StringExt
    {
        public static bool ContainsCaseInsensitive(this string text, string containedString)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(containedString))
                return false;

            return text.IndexOf(containedString, 0, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool StartsWithAny(this string text, IEnumerable<string> stringsToMatch)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text");

            if (stringsToMatch == null || !stringsToMatch.Any())
                throw new ArgumentException("stringsToMatch");

            foreach (var matchString in stringsToMatch)
            {
                if (text.StartsWith(matchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}