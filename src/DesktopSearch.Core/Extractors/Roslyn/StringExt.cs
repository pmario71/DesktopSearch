/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens AG/Siemens Medical Solutions USA, Inc., 2015. All rights reserved
   ------------------------------------------------------------------------------------------------- */
   
namespace CodeSearch.Extractors.Roslyn
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