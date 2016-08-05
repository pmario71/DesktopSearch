using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.DataModel.Code
{
    public class MethodDescriptor : IDescriptor, IAPIElement
    {
        public MethodDescriptor(string methodName, string parameters, string returnType, string filePath, int lineNR)
        {
            this.MethodName = methodName;
            this.Parameters = parameters;
            this.ReturnType = returnType;

            this.FilePath = filePath;
            this.LineNr = lineNR;
        }

        public string MethodName { get; private set; }

        public string Parameters { get; private set; }

        public string ReturnType { get; private set; }

        /// <summary>
        /// File in which the declaration was found
        /// </summary>
        public string FilePath { get; private set; }

        public int LineNr { get; set; }

        public API APIDefinition { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1})", this.MethodName, this.Parameters);
        }

        public MemberType Type { get { return MemberType.Method; } }
    }
}
