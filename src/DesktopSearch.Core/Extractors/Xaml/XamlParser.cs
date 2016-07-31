using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DesktopSearch.Core.Extractors.Xaml
{
    public class XamlParser
    {
        private const string clrNamespace = "clr-namespace:";
        private readonly Func<string, bool> _isIncluded;

        //static Regex _regex = new Regex(
        //                         "clr-namespace:(?<ns>[^;]*);assembly=(?<asm>[^;]*)",
        //                           RegexOptions.IgnoreCase
        //                           | RegexOptions.Multiline
        //                           | RegexOptions.CultureInvariant
        //                           | RegexOptions.IgnorePatternWhitespace
        //                           | RegexOptions.Compiled
        //                       );

        /// <summary>
        /// Create a new XamlParser instance. If a <see cref="assemblyNameWhiteList"/> is passed, only references that point to assemblies
        /// listed in the whitelist are returned. AssemblyNames do not need to be fully qualified. It is sufficient to provide only the
        /// significant part, e.g. syngo.Services.Workflow.
        /// </summary>
        /// <param name="assemblyNameWhiteList">[optional] assemblies whose name start with any of the entries in the list are included. All other
        /// assemblies are ignored.</param>
        public XamlParser(XamlParserConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration=new XamlParserConfiguration();
            }

            if (configuration.AssemblyNameWhitelist != null && configuration.AssemblyNameWhitelist.Any())
            {

                _isIncluded = s => s.StartsWithAny(configuration.AssemblyNameWhitelist);
            }
            else
            {
                // return all types
                _isIncluded = _ => true;
            }
        }


        /// <summary>
        /// Finds all *.xamlx files in <see cref="folderToParse"/> and extracts the types that are used.
        /// </summary>
        /// <remarks>List is filtered for types that are contained in assemblies starting with 'syngo.Services.Workflow'!</remarks>
        /// <param name="folderToParse"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public IEnumerable<TypeInfo> ExtractReferencedTypes(string folderToParse, out IEnumerable<ParseError> errors)
        {
            if (!Directory.Exists(folderToParse))
            {
                throw new ArgumentException( string.Format("Specified folder '{0}' does not exist!", folderToParse));
            }

            var list = new List<TypeInfo>();
            var parseErrors = new List<ParseError>();
            
            foreach (var file in Directory.EnumerateFiles(folderToParse, "*.xaml?", SearchOption.AllDirectories))
            {
                try
                {
                    var extractReferencedTypes = ExtractReferencedTypes(new FileInfo(file));
                    list.AddRange(extractReferencedTypes);
                }
                catch (Exception ex)
                {
                    var error = new ParseError()
                                {
                                    File = file,
                                    Exception = ex
                                };
                    parseErrors.Add(error);
                }
            }
            errors = parseErrors;
            return list;
        }


        /// <summary>
        /// Folds all entries referring to the same type into a single entry. List of FileReferences merged accordingly.
        /// </summary>
        /// <param name="typeInfos"></param>
        /// <returns></returns>
        public IEnumerable<TypeInfo> FoldTypeInfos(IEnumerable<TypeInfo> typeInfos)
        {
            if (typeInfos == null)
                throw new ArgumentNullException("typeInfos");

            var set = new Dictionary<int,TypeInfo>();
            foreach (var info in typeInfos)
            {
                TypeInfo found;
                if (set.TryGetValue(info.GetHashCode(), out found))
                {
                    found.FileReferences.AddRange(info.FileReferences);
                }
                else
                {
                    set.Add(info.GetHashCode(),info);
                }
            }
            return set.Values;
        }

        /// <summary>
        /// Extracts referenced type (name and assembly name) from a passed in xaml(x) file.
        /// </summary>
        /// <param name="xamlFile"></param>
        /// <returns></returns>
        public List<TypeInfo> ExtractReferencedTypes(FileInfo xamlFile)
        {
            XDocument doc = XDocument.Load(xamlFile.FullName, LoadOptions.SetLineInfo);

            var list = new List<TypeInfo>();
            foreach (var xElement in doc.Root.Descendants().Where(n => n.NodeType == XmlNodeType.Element))
            {
                string namespaceName = xElement.Name.NamespaceName;
                
                TypeInfo typeInfo;
                if (TryExtractTypeInfoFromXml(namespaceName, xElement.Name.LocalName, out typeInfo) &&
                    _isIncluded(typeInfo.AssemblyName))
                {
                    var reference = new FileReference(xamlFile.FullName, ((IXmlLineInfo)xElement).LineNumber);
                    typeInfo.FileReferences.Add(reference);
                    list.Add(typeInfo);
                }
            }
            return list;
        }

        /// <summary>
        /// Tries to extract <see cref="TypeInfo"/> (TypeName and Assembly) from information written in xml.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="typeNameFragment"></param>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static bool TryExtractTypeInfoFromXml(string namespaceName, string typeNameFragment, out TypeInfo typeInfo)
        {
            typeInfo = null;
            if (!namespaceName.StartsWith(clrNamespace))
                return false;

            int pos = namespaceName.IndexOf(';');

            try
            {
                if (pos >= 0)
                {
                    string typeName = string.Format("{0}.{1}", namespaceName.Substring(14, pos - 14), typeNameFragment);
                    string assemblyName = namespaceName.Substring(pos + 10);

                    typeInfo = new TypeInfo(typeName, assemblyName);

                    return true;
                }
                else
                {
                    // no assembly reference available
                    string typeName = string.Format("{0}.{1}", namespaceName.Substring(14), typeNameFragment);
                    typeInfo = new TypeInfo(typeName, "<local>");

                    return true;
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Failed to parse: {0} - {1}", namespaceName, typeNameFragment);
            }
            return false;
        }

        private static bool AssemblyInWorkflowModule(XAttribute arg)
        {
            return arg.Value.ContainsCaseInsensitive("assembly=syngo.Services.Workflow");
        }

        private static bool FilterSYReferences(XAttribute arg)
        {
            return arg.Value.ContainsCaseInsensitive("clr-namespace:");
        }
    }
}