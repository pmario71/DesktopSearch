/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2016. All rights reserved
   ------------------------------------------------------------------------------------------------- */
   
namespace CodeSearch.Extractors.Roslyn
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