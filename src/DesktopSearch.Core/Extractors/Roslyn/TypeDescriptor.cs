/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens AG/Siemens Medical Solutions USA, Inc., 2015. All rights reserved
   ------------------------------------------------------------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Diagnostics.Contracts;

namespace CodeSearch.Extractors.Roslyn
{
    public class TypeDescriptor : IAPIElement
    {
        private readonly Lazy<IList<IDescriptor>> _members = new Lazy<IList<IDescriptor>>(() => new List<IDescriptor>());

        /// <summary>
        /// Name shall not be null, @namespace can return null.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="name"></param>
        /// <param name="namespace"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNR"></param>
        public TypeDescriptor(ElementType elementType, string name, Visibility visibility,
            string @namespace, string filePath, int lineNR, string comment=null)
        {
            //Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            //Contract.Requires<ArgumentOutOfRangeException>(lineNR >= 0);

            Name        = name;
            Namespace   = @namespace;
            ElementType = elementType;
            Visibility  = visibility;

            FilePath    = filePath;
            LineNr      = lineNR;
            Comment     = comment;
        }

        /// <summary>
        /// Name of type.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Namespace of type. Can return null!
        /// </summary>
        public string Namespace { get; set; }
        public ElementType ElementType { get; private set; }
        public Visibility Visibility { get; set; }

        /// <summary>
        /// File in which the declaration was found
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Line number in file where declaration was found.
        /// </summary>
        public int LineNr { get; set; }

        public string Comment { get; private set; }

        public API APIDefinition { get; set; }

        public IList<IDescriptor> Members
        {
            get 
            {
                return this._members.Value; 
            }
        }
    }

    public enum Visibility
    {
        Public,
        Internal,
        Private,
    }
}