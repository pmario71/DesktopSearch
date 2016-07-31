using System.Collections.Generic;

namespace DesktopSearch.Core.Extractors.Xaml
{
    public class XamlParserConfiguration
    {
        private IEnumerable<string> _assemblyNameWhitelist;
        private IEnumerable<string> _extensionsToConsider;

        public IEnumerable<string> AssemblyNameWhitelist
        {
            get
            {
                if (this._assemblyNameWhitelist == null)
                {
                    _assemblyNameWhitelist = new List<string>();
                }
                return this._assemblyNameWhitelist;
            }
            set { this._assemblyNameWhitelist = value; }
        }

        public IEnumerable<string> ExtensionsToConsider
        {
            get
            {
                if (this._extensionsToConsider == null)
                {
                    _extensionsToConsider = new List<string>();
                }
                return this._extensionsToConsider;
            }
            set { this._extensionsToConsider = value; }
        }
    }
}