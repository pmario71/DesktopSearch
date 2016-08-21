using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.FileSystem
{
    /// <summary>
    /// Returns true, in case a file's extension is on the extension white list.
    /// </summary>
    public class ExcludeFileByExtensionFilter
    {
        private readonly HashSet<string> _map;

        public ExcludeFileByExtensionFilter(params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException("extensions");

            _map = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);
        }

        public bool FilterByExtension(string arg)
        {
            var ext = Path.GetExtension(arg);
            if (ext == null)
                return true;

            if (_map.Contains(ext.Substring(1)))
            {
                return false;
            }
            return true;
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
