/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2016. All rights reserved
   ------------------------------------------------------------------------------------------------- */

using System;
using System.Text.RegularExpressions;

namespace CodeSearch.Extractors.Roslyn
{
    internal class APIDefinitionExtractor
    {
        internal static API Parse(string comment)
        {
            if (comment == null)
            {
                return API.Undefined;
            }

            int pos = comment.IndexOf("API", StringComparison.OrdinalIgnoreCase);
            if (pos < 0)
                return API.Undefined;

            pos = comment.IndexOf(":", pos, StringComparison.OrdinalIgnoreCase);
            if (pos < 0)
                return API.Undefined;

            bool NotFound = true;
            while (NotFound && ++pos < comment.Length)
            {
                NotFound = char.IsWhiteSpace(comment[pos]);
            }

            if (NotFound)
            {
                return API.Undefined;
            }

            if (comment.Substring(pos).StartsWith("no", StringComparison.OrdinalIgnoreCase))
            {
                return API.No;
            }
            if (comment.Substring(pos).StartsWith("yes", StringComparison.OrdinalIgnoreCase))
            {
                return API.Yes;
            }

            return API.Undefined;
        }
    }
}