using CodeSearch.Extractors.Roslyn;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSearchTests.Indexing.Roslyn
{
    public class RoslynParser_APIFlag_extracted_from_methods_and_properties
    {

        [Fact]
        public void API_flag_extracted_from_comments()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text;
                                       namespace HelloWorld.Something 
                                       { 
                                           /// <summary>API:NO
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

            Assert.Equal(API.No, extractedTypes.First().APIDefinition);
        }

        [Fact]
        public void API_flag_extracted_from_property()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text;
                                       namespace HelloWorld.Something 
                                       { 
                                           /// <summary>API:NO
                                           /// This is a summary description.
                                           /// </summary>
                                           public class Program 
                                           { 
                                               /// <summary>API:YES</summary>
                                               public string File 
                                               { 
                                                  get { return null; }
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(API.Yes, extractedTypes.First().Members.First().APIDefinition);
        }

        [Fact]
        public void API_flag_extracted_from_method()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text;
                                       namespace HelloWorld.Something 
                                       { 
                                           /// <summary>API:NO
                                           /// This is a summary description.
                                           /// </summary>
                                           public class Program 
                                           { 
                                               /// <summary>API:YES</summary>
                                               public void File()
                                               { 
                                               } 
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(API.Yes, extractedTypes.First().Members.First().APIDefinition);
        }

        [Fact]
        public void API_flag_extracted_from_public_field()
        {
            const string csharp = @"   using System; using System.Collections.Generic; 
                                       using System.Text;
                                       namespace HelloWorld.Something 
                                       { 
                                           /// <summary>API:NO
                                           /// This is a summary description.
                                           /// </summary>
                                           public class Program 
                                           { 
                                               /// <summary>API:YES</summary>
                                               public string File = "";
                                           } 
                                       }";

            var parser = new RoslynParser();
            var extractedTypes = parser.ExtractTypes(csharp);

            Assert.Equal(API.Yes, extractedTypes.First().Members.First().APIDefinition);
        }
    }
}
