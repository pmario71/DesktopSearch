using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.DataModel.Documents
{
    public class DocDescriptor
    {
        public string Path { get; set; }

        public DateTime LastModified { get; set; }

        public string Content { get; set; }

        public override string ToString()
        {
            return $"{Path} -- {LastModified} --  {Content.Length}";
        }
    }
}
