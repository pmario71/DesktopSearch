using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Code;
using DesktopSearch.Core.Extractors.Roslyn;
using DesktopSearch.Core.Tests.Helper;
using ElasticSearch.Prototyping.Utils;
using Nest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Prototyping
{
    public class ElasticSearch_Indexing
    {
        private readonly string testDataPath = @"D:\Projects\GitHub\DesktopSearch\test\DesktopSearch.Core.Tests\TestData\IndexExtractors\Roslyn\APIClass.cs";
        const string indexName = "test_docsearch";

        public void Index_CaseInsensitive()
        {
            const string indexName = "test_codesearch";
            InstanceDescriptor instance = DockerControlClient.Start("elasticsearch", "-p 9200:9200").Result;

            try
            {
                var settings = new ConnectionSettings(new Uri(Configuration.ElasticSearchUri));

                settings.DefaultIndex(indexName);
                settings.DisableDirectStreaming()
                        .OnRequestCompleted(details =>
                        {
                            Console.WriteLine("### ES REQEUST ###");
                            if (details.RequestBodyInBytes != null) Console.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                            Console.WriteLine("### ES RESPONSE ###");
                            if (details.ResponseBodyInBytes != null) Console.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
                        })
                        .PrettyJson();

                var elastic = new ElasticClient(settings);

                if (!elastic.IndexExists(indexName).Exists)
                {
                    var indexDescriptor = new CreateIndexDescriptor(indexName)
                        .Mappings(ms => ms
                            .Map<TypeDescriptor>(m => m.AutoMap())
                            .Map<MethodDescriptor>(m => m.AutoMap())
                            .Map<FieldDescriptor>(m => m.AutoMap()));

                    elastic.CreateIndex(indexName, i => indexDescriptor);

                    var parser = new RoslynParser();
                    var extractedTypes = parser.ExtractTypes(File.ReadAllText(testDataPath));

                    var index = elastic.IndexMany(extractedTypes);

                    Console.WriteLine(index);
                }

                // dump indices
                // -----------------------------------------------
                //DumpIndices(elastic);

                var searchResults = elastic.Search<TypeDescriptor>(x => x.Query(q => q.Wildcard("name", "api*")));
                //.MultiMatch(m => m
                //    .Query("Mart")
                //        .Fields(f => f
                //        .Fields(f1 => f1.Lastname, f2 => f2.Firstname)))));

                Console.WriteLine("Searching ...");
                Console.WriteLine($"{searchResults.DebugInformation}");
                Console.WriteLine("-------------------------------------------");

                if (searchResults.Documents.Any())
                {
                    searchResults.Documents.Dump();
                }
                else
                {
                    Console.WriteLine("<no results found!>");
                }

                //Assert.Equal(1, searchResults.Hits.Count());
                //Assert.NotNull(searchResults.Documents.FirstOrDefault(f => string.Compare(f.Lastname, "Laarman", StringComparison.OrdinalIgnoreCase) == 0));

                //Assert.False(true, "Just to see the output!");
                Console.ReadLine();
            }
            finally
            {
                //await DockerControlClient.Stop(instance);
            }
        }

        public void SearchOnly()
        {
            ConnectionSettings settings = InitializeSettings(indexName);
            var elastic = new ElasticClient(settings);

            var searchResults = elastic.Search<DocDescriptor>(x => x.Query(q => q.QueryString(q2 => q2.Query("telefon"))));

            Console.WriteLine("Searching ...");
            Console.WriteLine("-------------------------------------------");

            if (searchResults.Documents.Any())
            {
                //searchResults.Documents.Dump();
                var d = searchResults.Documents.First();

                Console.WriteLine($"Hits: {searchResults.Hits.Count()}");
                Console.WriteLine($"Path: {searchResults.Hits.First().Source.Path}");
            }
            else
            {
                Console.WriteLine("<no results found!>");
            }
            Console.ReadLine();
        }

        public void Document()
        {
            bool useStatic = false;
            string path = @"D:\Downloads\ct.15.22.126-129.pdf";

            const string staticContent = "e1xydGYxXGFuc2kNCkxvcmVtIGlwc3VtIGRvbG9yIHNpdCBhbWV0DQpccGFyIH0=";
            var base64Content = (useStatic) ? staticContent :  Convert.ToBase64String(File.ReadAllBytes(path));

            try
            {
                ConnectionSettings settings = InitializeSettings(indexName);

                var elastic = new ElasticClient(settings);

                if (!elastic.IndexExists(indexName).Exists)
                {
                    CreateIndex(indexName, elastic);
                }
                // --------------------------------------------------------------------
                // store content
                // --------------------------------------------------------------------
                

                var docs = new[]
                {
                    new DocDescriptor()
                    {
                        Path = "static content",
                        Content = staticContent
                    },
                    new DocDescriptor()
                    {
                        Path = "normal text file",
                        Content = Convert.ToBase64String(File.ReadAllBytes(@"D:\Projects\GitHub\DesktopSearch\test\ElasticSearch.Prototyping\ElasticSearch_Indexing.cs")),
                    },
                    new DocDescriptor()
                    {
                        Path = "pdf file",
                        Content = base64Content
                    }
                };

                var index = elastic.Bulk(b => b.IndexMany(docs));

                Console.WriteLine(index);

                // dump indices
                // -----------------------------------------------
                //DumpIndices(elastic);

                // examples
                // https://www.elastic.co/guide/en/elasticsearch/plugins/master/mapper-attachments-helloworld.html

                //var searchResults = elastic.Search<DocDescriptor>(x => x.Query(q => q.Wildcard("path", "*")));
                var searchResults = elastic.Search<DocDescriptor>(x => x.Query(q => q.QueryString(q2 => q2.Query("ipsum"))));

                Console.WriteLine("Searching ...");
                //Console.WriteLine($"{searchResults.DebugInformation}");
                Console.WriteLine("-------------------------------------------");

                //TODO:  Plugin seems not to be installed!

                if (searchResults.Documents.Any())
                {
                    //searchResults.Documents.Dump();
                    var d = searchResults.Documents.First();

                    var areEqual = string.Compare(d.Content, base64Content) == 0;

                    var c = Console.ForegroundColor;
                    Console.ForegroundColor = (areEqual) ? c : ConsoleColor.Red;
                    Console.WriteLine($"Documents are the same: {areEqual}");
                    Console.ForegroundColor = c;
                }
                else
                {
                    Console.WriteLine("<no results found!>");
                }
                Console.ReadLine();
            }
            finally
            {
                //await DockerControlClient.Stop(instance);
            }
        }

        private static ConnectionSettings InitializeSettings(string indexName)
        {
            var settings = new ConnectionSettings(new Uri(Configuration.ElasticSearchUri));

            settings.DefaultIndex(indexName);
            settings.DisableDirectStreaming()
               .OnRequestCompleted(details =>
               {
                   Console.WriteLine("### ES REQEUST ###");
                   if (details.RequestBodyInBytes != null)
                       Console.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                   //    Console.WriteLine("### ES RESPONSE ###");
                   //    if (details.ResponseBodyInBytes != null) Console.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
               })
            .PrettyJson();
            return settings;
        }

        private static void CreateIndex(string indexName, ElasticClient elastic)
        {
            // --------------------------------------------------------------------
            // setup index
            // --------------------------------------------------------------------
            var indexDescriptor = new CreateIndexDescriptor(indexName);

            indexDescriptor.Mappings(mp => mp.Map<DocDescriptor>(m => m
                .AutoMap()
                .Properties(ps => ps
                        .String(s => s
                            .Name(f => f.Path)
                            .Index(FieldIndexOption.Analyzed)
                            .Store(true))
                .Attachment(atm => atm
                        .Name(p => p.Content)
                        .FileField(f => f
                                .Name(p => p.Content)
                                .Index(FieldIndexOption.Analyzed)
                                .Store(true)
                                .TermVector(TermVectorOption.WithPositionsOffsets))))));

            elastic.CreateIndex(indexName, i => indexDescriptor);
        }

        private static void DumpIndices(ElasticClient elastic)
        {
            var stats = elastic.IndicesStats(Indices.All);
            foreach (var idx in stats.Indices)
            {
                Console.WriteLine($"{idx.Key}  -> Docs: {idx.Value.Total.Documents.Count}");
            }
        }
    }

    public class DocDescriptor
    {
        public string Path { get; set; }

        [Attachment(Store = true)]
        public string Content { get; set; }

        public override string ToString()
        {
            return $"{Path}  --  {Content.Length}";
        }
    }

    public static class MyClass
    {
        public static void Dump(this IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

}
