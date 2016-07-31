using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.DataModel
{
    public interface IAPIElement
    {
        API APIDefinition { get; set; }
    }

    public interface IDescriptor : IAPIElement
    {
        MemberType Type { get; }
    }
}
