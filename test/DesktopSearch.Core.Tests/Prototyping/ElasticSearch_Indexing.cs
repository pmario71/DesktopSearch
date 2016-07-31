using DesktopSearch.Core.Tests.Helper;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DesktopSearch.Core.Tests.Prototyping
{
    public class ElasticSearch_Indexing
    {
        private readonly ITestOutputHelper _output;

        public ElasticSearch_Indexing(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        [Trait("Category","Explicit")]
        public void Index_CaseInsensitive()
        {
            InstanceDescriptor instance = DockerControlClient.Start("elasticsearch", "-p 9200:9200").Result;

            try
            {
                var settings = new ConnectionSettings(new Uri(Configuration.ElasticSearch.Uri));
                settings.DefaultIndex(Configuration.CodeSearch.IndexName);
                settings.DisableDirectStreaming()
                        .OnRequestCompleted(details =>
                        {
                            _output.WriteLine("### ES REQEUST ###");
                            if (details.RequestBodyInBytes != null) _output.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                            _output.WriteLine("### ES RESPONSE ###");
                            if (details.ResponseBodyInBytes != null) _output.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
                        })
                        .PrettyJson();

                var elastic = new ElasticClient(settings);

                if (!elastic.IndexExists(Configuration.CodeSearch.IndexName).Exists)
                {
                    var indexDescriptor = new CreateIndexDescriptor(Configuration.CodeSearch.IndexName)
                        .Mappings(ms => ms
                            .Map<Person>(m => m.AutoMap()));

                    elastic.CreateIndex(Configuration.CodeSearch.IndexName, i => indexDescriptor);

                    var person = new Person
                    {
                        Id = "1",
                        Firstname = "Martijn",
                        Lastname = "Laarman"
                    };

                    var index = elastic.Index(person);
                }

                // dump indices
                // -----------------------------------------------
                var stats = elastic.IndicesStats(Indices.All);
                foreach (var idx in stats.Indices)
                {
                    Console.WriteLine($"{idx.Key}  -> Docs: {idx.Value.Total.Documents.Count}");
                }

                var searchRequest = new SearchRequest
                {
                    //From = 0,
                    //Size = 10,
                    Query = new TermQuery
                    {
                        Field = "Firstname",
                        Value = "Martijn"
                    },
                    Explain = true
                };

                var searchResults = elastic.Search<Person>(x => x.Query(q => q.Wildcard("firstname","mar*")));
                                                                    //.MultiMatch(m => m
                                                                    //    .Query("Mart")
                                                                    //        .Fields(f => f
                                                                    //        .Fields(f1 => f1.Lastname, f2 => f2.Firstname)))));

                _output.WriteLine("Searching ...");
                _output.WriteLine($"{searchResults.DebugInformation}");
                _output.WriteLine("-------------------------------------------");
                searchResults.Documents.Dump(_output);

                Assert.Equal(1, searchResults.Hits.Count());
                Assert.NotNull(searchResults.Documents.FirstOrDefault(f => string.Compare(f.Lastname, "Laarman", StringComparison.OrdinalIgnoreCase) == 0));

                Assert.False(true, "Just to see the output!");
            }
            finally
            {
                //await DockerControlClient.Stop(instance);
            }
        }

    }

    public class Person
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public override string ToString()
        {
            return $"{Id}  -- {Lastname}  --  {Firstname}";
        }
    }

    public static class MyClass
    {
        public static void Dump(this IEnumerable<Person> persons, ITestOutputHelper output)
        {
            foreach (var item in persons)
            {
                output.WriteLine(item.ToString());
            }
        }
    }

}
