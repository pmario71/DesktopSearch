using System.Linq;
using Xunit;
using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.Extractors.Roslyn;
using DesktopSearch.Core.DataModel.Code;

namespace CodeSearchTests.Indexing.Roslyn
{
    
    public class RoslynParser_DefinedTypes_Tests
    {
        [Fact]
        public void Classes_are_parsed()
        {
            const string csharp = @"   namespace HelloWorld.Something 
                                       { 
                                           public class TestClass
                                           { 
                                               public void Method()
                                               {
                                               }
                                           } 
                                       }";

            var parser = new RoslynParser();
            var APIMethods = parser.ExtractTypes(csharp);

            Assert.Equal(1, APIMethods.Count());

            var typeDescriptor = APIMethods.First();
            Assert.Equal("TestClass", typeDescriptor.Name);
            Assert.Equal("HelloWorld.Something", typeDescriptor.Namespace);
            Assert.Equal(ElementType.Class, typeDescriptor.ElementType);
        }

        [Fact]
        public void Enums_are_parsed()
        {
            const string csharp = @"   namespace HelloWorld.Something 
                                       { 
                                           public enum TestEnum 
                                           { 
                                               Value1,
                                               Value2
                                           } 
                                       }";

            var parser = new RoslynParser();
            var APIMethods = parser.ExtractTypes(csharp);

            Assert.Equal(1, APIMethods.Count());
            
            var typeDescriptor = APIMethods.First();
            Assert.Equal("TestEnum", typeDescriptor.Name);
            Assert.Equal("HelloWorld.Something", typeDescriptor.Namespace);
            Assert.Equal(ElementType.Enum, typeDescriptor.ElementType);
        }

        [Fact]
        public void Interfaces_are_parsed()
        {
            const string csharp = @"   namespace HelloWorld.Something 
                                       { 
                                           public interface ITest
                                           { 
                                               void Method();
                                           } 
                                       }";

            var parser = new RoslynParser();
            var APIMethods = parser.ExtractTypes(csharp);

            Assert.Equal(1, APIMethods.Count());
            
            var typeDescriptor = APIMethods.First();
            Assert.Equal("ITest", typeDescriptor.Name);
            Assert.Equal("HelloWorld.Something", typeDescriptor.Namespace);
            Assert.Equal(ElementType.Interface, typeDescriptor.ElementType);
        }

        [Fact]
        public void Structs_are_parsed()
        {
            const string csharp = @"   namespace HelloWorld.Something 
                                       { 
                                           public struct TestStruct
                                           { 
                                               void Method();
                                           } 
                                       }";

            var parser = new RoslynParser();
            var APIMethods = parser.ExtractTypes(csharp);

            Assert.Equal(1, APIMethods.Count());

            var typeDescriptor = APIMethods.First();
            Assert.Equal("TestStruct", typeDescriptor.Name);
            Assert.Equal("HelloWorld.Something", typeDescriptor.Namespace);
            Assert.Equal(ElementType.Struct, typeDescriptor.ElementType);
        }
    }
}