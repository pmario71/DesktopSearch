using DesktopSearch.Core.DataModel;
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

        public void Document()
        {
            const string indexName = "test_docsearch";
            InstanceDescriptor instance = DockerControlClient.Start("elasticsearch", "-p 9200:9200").Result;

            try
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

                var elastic = new ElasticClient(settings);

                string path = @"D:\Downloads\ct.15.22.126-129.pdf";
                if (!elastic.IndexExists(indexName).Exists)
                {
                    var indexDescriptor = new CreateIndexDescriptor(indexName)
                        .Mappings(ms => ms
                            .Map<DocDescriptor>(m => m.AutoMap()));

                    elastic.CreateIndex(indexName, i => indexDescriptor);
                    
                    var doc = new DocDescriptor() { Path = path, Content = File.ReadAllBytes(path) };

                    var index = elastic.Index(doc);

                    Console.WriteLine(index);
                }

                // dump indices
                // -----------------------------------------------
                //DumpIndices(elastic);

                // examples
                // https://www.elastic.co/guide/en/elasticsearch/plugins/master/mapper-attachments-helloworld.html

                //var searchResults = elastic.Search<DocDescriptor>(x => x.Query(q => q.Wildcard("path", "*")));
                var searchResults = elastic.Search<DocDescriptor>(x => x.Query(q => q.QueryString(i => i.Query("Telefonrechnungen"))));

                Console.WriteLine("Searching ...");
                //Console.WriteLine($"{searchResults.DebugInformation}");
                Console.WriteLine("-------------------------------------------");

                //TODO:  Plugin seems not to be installed!

                if (searchResults.Documents.Any())
                {
                    //searchResults.Documents.Dump();
                    var d = searchResults.Documents.First();

                    var areEqual = ByteComparer.ByteArrayCompare(d.Content, File.ReadAllBytes(path));

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
        public byte[] Content { get; set; }

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
