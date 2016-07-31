using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSearch.Extractors.Roslyn;
using Xunit;
using Directory = System.IO.Directory;
using System.Reflection;
using DesktopSearch.Core.DataModel;

namespace CodeSearchTests.Indexing.Roslyn
{
    
    public class RoslynParserTests
    {
        //TODO:  copying files into output directory does not work, therefore we need to go for fully qualified path
        private const string testDataPath = @"D:\Projects\GitHub\DesktopSearch\test\DesktopSearch.Core.Tests\TestData\IndexExtractors\Roslyn";
            //"Indexing\\Roslyn\\TestData";

        public RoslynParserTests()
        {
            
        }

        [Fact(Skip="Requires reflection")]
        public void GetNamespaceMembers()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text; 
                                       namespace HelloWorld 
                                       { 
                                           class Program 
                                           { 
                                               static void Main(string[] args) 
                                               { 
                                                   Console.WriteLine(""Hello, World!""); 
                                               } 
                                           } 
                                       }";
            //RoslynParser.GetNamespaceMembers(csharp);
        }

        [Fact]
        public void Non_public_methods_are_ignored()
        {
            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(File.ReadAllText(string.Format("{0}\\APIClass.cs", testDataPath)));

            Assert.Equal(1, extractedTypes.Count());
            Assert.Equal(2, extractedTypes.First().Members.Count());
        }

        [Fact]
        public void ReturnType_extracted()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text; 
                                       namespace HelloWorld 
                                       { 
                                           public class Program 
                                           { 
                                               public string File() 
                                               { 
                                                  return "";
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(1, extractedTypes.Count());

            var memberDescriptor = extractedTypes.First().Members.First() as MethodDescriptor;
            Assert.Equal("File", memberDescriptor.MethodName);
            Assert.Equal("string", memberDescriptor.ReturnType);
        }

        [Fact]
        public void Public_properties_extracted()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text; 
                                       namespace HelloWorld 
                                       { 
                                           public class Program 
                                           { 
                                               public string File 
                                               { 
                                                  get { return null; }
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(1, extractedTypes.Count());

            var fieldDescriptor = extractedTypes.First().Members.First() as FieldDescriptor;
            Assert.Equal("File", fieldDescriptor.Name);
            Assert.Equal("string", fieldDescriptor.FieldType);
        }

        [Fact]
        public void Public_fields_extracted()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text; 
                                       namespace HelloWorld 
                                       { 
                                           public class Program 
                                           { 
                                               public string File;
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(1, extractedTypes.Count());

            var fieldDescriptor = extractedTypes.First().Members.First() as FieldDescriptor;
            Assert.Equal("File", fieldDescriptor.Name);
            Assert.Equal("string", fieldDescriptor.FieldType);
        }

        [Fact]
        public void Namespaces_resolved_correctly()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text; 
                                       namespace HelloWorld.Something 
                                       { 
                                           public class Program 
                                           { 
                                               public string File 
                                               { 
                                                  get { return null; }
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(1, extractedTypes.Count());
            Assert.Equal("HelloWorld.Something", extractedTypes.First().Namespace);
        }

        [Fact]
        public void Linenumber_resolved_correctly()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text; 
                                       namespace HelloWorld.Something 
                                       { 
                                           public class Program 
                                           { 
                                               public string File 
                                               { 
                                                  get { return null; }
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(1, extractedTypes.Count());
            Assert.Equal(4, extractedTypes.First().LineNr);
        }

        [Fact]
        public void Namespaces_are_trimmed_and_dot_appended()
        {
            const string ns = "SomeNamespace.Test \r\n";
            Assert.Equal("SomeNamespace.Test", ns.PrepareNamespace());
        }


        [Fact]
        public void FilePath_is_extracted()
        {
            string[] files =
            {
                string.Format("{0}\\AllMembersIgnoredClass.cs", testDataPath),
                string.Format("{0}\\APIClass.cs", testDataPath),
            };

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(files);

            Assert.Equal(1, extractedTypes.Count());
            Assert.Equal(files[1], extractedTypes.First().FilePath);
        }

        [Fact]
        public void Tester_namespaces_are_ignored()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text; 
                                       namespace HelloWorld.Test.Something 
                                       { 
                                           public class Program 
                                           { 
                                               public string File 
                                               { 
                                                  get { return null; }
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(0, extractedTypes.Count());
        }

        [Fact]
        public void Class_comments_are_extracted()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text;
                                       namespace HelloWorld.Something 
                                       { 
                                           /// <summary>
                                           /// This is a summary description.
                                           /// </summary>
                                           public class Program 
                                           { 
                                               public string File 
                                               { 
                                                  get { return null; }
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.NotEmpty(extractedTypes.First().Comment);
        }

        [Fact]
        public void Interface_comments_are_extracted()
        {
            const string csharp = @"   /// <apiflag>No</apiflag>
                                       /// <summary> API:NO
                                       /// Interface for accessing the IsPatientMixUp property of Requests
                                       /// </summary>
                                       internal interface IRequestWithPatientMixup
                                       {
                                           bool IsPatientMixUp { get; set; }
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.NotEmpty(extractedTypes.First().Comment);
        }

        [Fact]
        public void Enum_comments_are_extracted()
        {
            const string csharp = @"   /// <apiflag>No</apiflag>
                                       /// <summary> API:NO
                                       /// Interface for accessing the IsPatientMixUp property of Requests
                                       /// </summary>
                                       public enum IRequestWithPatientMixup
                                       {
                                           Test,
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.NotEmpty(extractedTypes.First().Comment);
        }

        //[Fact(Skip="Access to drives not supported by .net core")]
        //public void PerformanceTest()
        //{
        //    string folderToParse = @"D:\Projects\syngo\DataModule\Main";
        //    if (!Directory.Exists(folderToParse))
        //    {
        //        folderToParse = @"E:\TFS_src\Mod\Data";
        //    }

        //    IEnumerable<string> files = Directory.GetFiles(folderToParse, "*", SearchOption.AllDirectories).FilterByExtension();
            
        //    var parser = new RoslynParser();

        //    var sw = Stopwatch.StartNew();
        //    var extractedTypes = parser.ExtractTypes(files);
        //    sw.Stop();

        //    Console.WriteLine("nr of extracted types: {0}", extractedTypes.Count());
        //    Console.WriteLine("extraction took      : {0} s", sw.Elapsed.TotalSeconds);

        //    sw = Stopwatch.StartNew();
        //    var typeDescriptors = extractedTypes.Where(t => t.Name.Contains("IDataCacheManager"));
        //    sw.Stop();

        //    foreach (var typeDescriptor in typeDescriptors)
        //    {
        //        Console.WriteLine("found: {0} - {1} - {2}", Enum.GetName(typeof(ElementType), typeDescriptor.ElementType), typeDescriptor.Name, typeDescriptor.FilePath);    
        //    }
            
        //    Console.WriteLine("finding took: {0} [ms]", sw.ElapsedMilliseconds);


        //    Console.WriteLine("\r\nStoring types to Lucene");
        //    var luceneHost = new LuceneHost(new RAMDirectory());
            
        //    sw.Start();
        //    luceneHost.IndexTypes(extractedTypes);
        //    sw.Stop();
        //    Console.WriteLine("storing to Lucene took: {0} [ms]", sw.ElapsedMilliseconds);

        //    sw.Start();
        //    // asterix in front because name contains namespace
        //    var indexedDocuments = luceneHost.Search("*IDataCacheManager*");
        //    sw.Stop();
        //    Console.WriteLine("Search in Lucene took: {0} [ms]", sw.ElapsedMilliseconds);

        //    foreach (var doc in indexedDocuments)
        //    {
        //        doc.Dump();
        //    }
            
        //    luceneHost.Dispose();
        //}
    }
}
