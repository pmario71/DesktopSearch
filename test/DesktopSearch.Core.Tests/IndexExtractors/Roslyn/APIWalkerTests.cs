using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace CodeSearchTests.Indexing.Roslyn
{
    public class APIWalkerTests
    {

        [Fact]
        public void RoslynAPI_prototyping_get_type_of_field()
        {

        }

    }

    public class RoslynAPI_Prototyping
    {
        //if (!classDeclaration.Modifiers.Any(SyntaxKind.PublicKeyword))

        [Fact]
        public void Get_visibility_of_class()
        {
            var classDoc = @"public class SomeClass
            {
                private SomeOtherClass someOtherClass;
            }";

            SyntaxTree classTree = SyntaxFactory.ParseSyntaxTree(classDoc);
            var classDecl = (ClassDeclarationSyntax)classTree.GetRoot().DescendantNodes().First(d => d is ClassDeclarationSyntax);

            Assert.True(classDecl.Modifiers.Any(t => t.IsKind(SyntaxKind.PublicKeyword) || t.IsKind(SyntaxKind.InternalKeyword)));
        }

        [Fact]
        public void Get_type_of_field()
        {
            var classDoc = @"public class SomeClass
            {
                private SomeOtherClass someOtherClass;
            }";

            SyntaxTree classTree = SyntaxFactory.ParseSyntaxTree(classDoc);
            var classDecl = (ClassDeclarationSyntax)classTree.GetRoot().DescendantNodes().First(d => d is ClassDeclarationSyntax);
            var field = classDecl.Members.OfType<FieldDeclarationSyntax>().First();
            var fieldType = field.Declaration.Type;

            Assert.Equal("SomeOtherClass", fieldType.ToString());
        }

        [Fact]
        public void Get_type_of_property()
        {
            var classDoc = @"public class SomeClass
            {
                public SomeOtherClass someOtherClass { get; };
            }";

            SyntaxTree classTree = SyntaxFactory.ParseSyntaxTree(classDoc);
            var classDecl = (ClassDeclarationSyntax)classTree.GetRoot().DescendantNodes().First(d => d is ClassDeclarationSyntax);
            var field = classDecl.Members.OfType<PropertyDeclarationSyntax>().First();
            var fieldType = field.Type;

            Assert.Equal("SomeOtherClass", fieldType.ToString());
        }
    }
}
