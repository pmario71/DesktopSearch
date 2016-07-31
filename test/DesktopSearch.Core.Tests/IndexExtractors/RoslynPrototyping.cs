using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;
using DesktopSearch.Core.Extractors.Roslyn;

namespace CodeSearchTests.Indexing
{
    public class RoslynPrototyping
    {
        [Fact]
        public void SingleLineComment()
        {
            var classDecl = Walker.Create(@"
/// <summary>This is an xml doc comment</summary>
class C
{
}");

            var extractComment = APIWalker.ExtractComment(classDecl);

            Assert.Equal("<summary>This is an xml doc comment</summary>", extractComment);
        }

        [Fact]
        public void SingleLineComment_multiple_lines()
        {
            var classDecl = Walker.Create(@"  /// <summary>This is an xml doc comment.
   /// the second line.</summary>
   class C
   {
   }");

            var extractComment = APIWalker.ExtractComment(classDecl);

            Assert.Equal("<summary>This is an xml doc comment.\r\nthe second line.</summary>", extractComment);
        }

        class Walker : SyntaxWalker
        {
            public static ClassDeclarationSyntax Create(string sourcecode)
            {
                var tree = CSharpSyntaxTree.ParseText(sourcecode);

                var root = (CompilationUnitSyntax)tree.GetRoot();

                var walker = new Walker();
                walker.Visit(root);
                Assert.NotNull(walker.Node);
                return walker.Node;
            }

            private ClassDeclarationSyntax _node;

            public ClassDeclarationSyntax Node
            {
                get { return _node; }
            }

            public override void Visit(SyntaxNode node)
            {
                if (node.IsKind(SyntaxKind.ClassDeclaration) && node.HasLeadingTrivia)
                {
                    _node = node as ClassDeclarationSyntax;
                    return;
                }
                base.Visit(node);
            }
        }
    }
}
