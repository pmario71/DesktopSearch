namespace DesktopSearch.Core.Extractors.Roslyn
{
    public static class StringExt
    {
        public static string PrepareNamespace(this string ns)
        {
            if (ns == null)
                return "<unknown>";

            return ns.Trim(new[] { ' ', ';', '\r', '\n' });
        }
    }
}