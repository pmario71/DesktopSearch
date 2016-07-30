using System;
using System.Collections.Generic;
using System.IO;

namespace DesktopSearch.Core.FileSystem
{
    internal static class FileFilter
    {
        private static readonly HashSet<string> _map = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                                                              {
                                                                  ".cs",
                                                              };

        public static bool FilterByExtension(this string arg)
        {
            var ext = Path.GetExtension(arg);
            if (ext == null)
                return false;

            if (_map.Contains(ext))
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<string> FilterByExtension(this IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file);
                if (ext == null)
                    continue;

                if (_map.Contains(ext))
                {
                    yield return file;
                }
            }
        }
    }
}