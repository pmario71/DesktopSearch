using System;
using System.Collections.Generic;
using System.IO;

namespace DesktopSearch.Core.FileSystem
{
    /// <summary>
    /// Returns true, in case a file's extension is on the extension white list.
    /// </summary>
    public class IncludeFileByExtensionFilter
    {
        private readonly HashSet<string> _map = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                                                              {
                                                                  ".cs",
                                                              };

        public IncludeFileByExtensionFilter(params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");

            _map = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);
        }

        public bool FilterByExtension(string arg)
        {
            var ext = Path.GetExtension(arg);
            if (ext == null)
                return false;

            if (_map.Contains(ext.Substring(1)))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<string> FilterByExtension(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                if (FilterByExtension(file))
                {
                    yield return file;
                }
            }
        }
    }
}