using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Code;

namespace DesktopSearch.Core.Extractors.Roslyn
{
    public class RoslynParser
    {
        private bool _ignoreTesterNamespaces = true;

        public bool IgnoreTesterNamespaces
        {
            get { return this._ignoreTesterNamespaces; }
            set { _ignoreTesterNamespaces = value; }
        }

        //public static void GetNamespaceMembers(string csharp)
        //{
        //    SyntaxTree tree = CSharpSyntaxTree.ParseText(csharp);

        //    var root = (CompilationUnitSyntax)tree.GetRoot();
        //    var compilation = CSharpCompilation.Create("HelloWorld")
        //                            .AddReferences(new MetadataReference[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) })
        //                            .AddSyntaxTrees(tree);

        //    var model = compilation.GetSemanticModel(tree, true);
        //    var nameInfo = model.GetSymbolInfo(root.Usings[0].Name);
        //    var systemSymbol = (INamespaceSymbol)nameInfo.Symbol;

        //    foreach (var ns in systemSymbol.GetNamespaceMembers())
        //    {
        //        Console.WriteLine(ns.Name);
        //    }

        //    var helloWorldString = root.DescendantNodes().OfType<LiteralExpressionSyntax>().First();
        //    Console.WriteLine(helloWorldString);
        //}

        public IEnumerable<TypeDescriptor> ExtractTypes(IEnumerable<string> filePaths)
        {
            var collection = new List<TypeDescriptor>();

            //var filteredPaths = (_ignoreTesterNamespaces) ?
            //                        filePaths.Where(s => s.IndexOf("test", StringComparison.InvariantCultureIgnoreCase) < 0) :
            //                        filePaths;


            var result = Parallel.ForEach(filePaths, p =>
            {
                SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(p));
                var root = (CompilationUnitSyntax)tree.GetRoot();

                var walker = new APIWalker(p, _ignoreTesterNamespaces);
                walker.Visit(root);
                lock (collection)
                {
                    collection.AddRange(walker.ParsedTypes);
                }
            });
            return collection;
        }

        public IEnumerable<TypeDescriptor> ExtractTypes(string csharp, string filePath = "<none>")
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(csharp);

            var root = (CompilationUnitSyntax)tree.GetRoot();

            var walker = new APIWalker(filePath, _ignoreTesterNamespaces);
            walker.Visit(root);

            return walker.ParsedTypes;
        }
    }
}
