using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.ElasticSearch;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Diagnostics.Contracts;

namespace DesktopSearch.Core.DataModel.Code
{
    public class TypeDescriptor : IAPIElement
    {
        private IList<IDescriptor> _members;

        /// <summary>
        /// Name shall not be null, @namespace can return null.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="name"></param>
        /// <param name="namespace"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNR"></param>
        public TypeDescriptor(ElementType elementType, string name, Visibility visibility,
            string @namespace, string filePath, int lineNR, string comment = null)
        {
            //Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            //Contract.Requires<ArgumentOutOfRangeException>(lineNR >= 0);

            Name = name;
            Namespace = @namespace;
            ElementType = elementType;
            Visibility = visibility;

            FilePath = filePath;
            LineNr = lineNR;
            Comment = comment;
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

        //[JsonProperty(TypeNameHandling = TypeNameHandling.Arrays)]
        [JsonConverter(typeof(CustomIDescriptorConverter))]
        //[ElasticProperty(Index = FieldIndexOption.NotAnalyzed, Store = true, IncludeInAll = false)]
        public IList<IDescriptor> Members
        {
            get 
            {
                if (_members == null)
                {
                    _members = new List<IDescriptor>();
                }
                return this._members; 
            }
        }

        //IList<MethodDescriptor> _methods;
        //public IList<MethodDescriptor> Methods
        //{
        //    get
        //    {
        //        if (_methods == null)
        //        {
        //            if (_members.IsValueCreated)
        //            {

        //                _methods = _members.Value.Where(t => t.Type == MemberType.Method).Cast<MethodDescriptor>().ToList();
        //                return _methods;
        //            }
        //        }
        //        return null;
        //    }
        //    //set
        //    //{
        //    //    //if (_members.IsValueCreated)
        //    //    //{
        //    //    //    _members.Value.Where(t => t.Type != MemberType.Method)
        //    //    //}
        //    //    _methods = value;
        //    //    foreach (var item in value)
        //    //    {
        //    //        _members.Value.Add(item);
        //    //    }
        //    //}
        //}
    }

    public enum Visibility
    {
        Public,
        Internal,
        Private,
    }
}