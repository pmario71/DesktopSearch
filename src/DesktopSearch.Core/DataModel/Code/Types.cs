using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.DataModel.Code
{
    

    public enum API
    {
        Undefined,
        No,
        Yes,
    }

    //public class CodeItem
    //{
    //    public string Name { get; set; }
    //    public string Namespace { get; set; }
    //    public int LineNR { get; set; }

    //    public ElementType ElementType { get; set; }

    //    public string Comment { get; set; }
    //}

    public enum ElementType
    {
        Class,
        Interface,
        Enum,
        Struct,
        Activity
    }

    public enum MemberType
    {
        Field,
        Property,
        Method,
    }
}
