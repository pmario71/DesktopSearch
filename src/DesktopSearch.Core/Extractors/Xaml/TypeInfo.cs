using System.Collections.Generic;

namespace DesktopSearch.Core.Extractors.Xaml
{
    public class TypeInfo
    {
        private readonly string _typeName;
        private readonly string _assemblyName;
        private List<FileReference> _fileReferences;

        public TypeInfo(string typeName, string assemblyName)
        {
            this._typeName = typeName;
            this._assemblyName = assemblyName;
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        public string AssemblyName
        {
            get { return _assemblyName; }
        }

        public List<FileReference> FileReferences
        {
            get
            {
                if (_fileReferences == null)
                {
                    lock(this)
                    {
                        if (_fileReferences == null)
                        {
                            _fileReferences = new List<FileReference>();
                        }
                    }
                }
                return this._fileReferences;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            TypeInfo p = obj as TypeInfo;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if TypeName matches
            return (this.GetHashCode() == p.GetHashCode());
        }

        public override int GetHashCode()
        {
            int hcTypeName = (this.TypeName != null) ? this.TypeName.GetHashCode() : -1;
            int hcAssemblyName = (this.AssemblyName != null) ? this.AssemblyName.GetHashCode() : -2;

            return hcTypeName ^ hcAssemblyName;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.TypeName, this.AssemblyName);
        }
    }
}