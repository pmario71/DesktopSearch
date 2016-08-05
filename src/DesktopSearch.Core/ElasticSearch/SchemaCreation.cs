using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Documents;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.ElasticSearch
{
    public class SchemaCreation
    {


        public async Task Initialize()
        {
            var settings = new ConnectionSettings(new Uri(Configuration.ElasticSearch.Uri));
            settings.DefaultIndex(Configuration.CodeSearch.IndexName);

            var elastic = new ElasticClient(settings);

            var res = await elastic.IndexExistsAsync(Configuration.CodeSearch.IndexName);

            if (res.IsValid && res.Exists)
                return;

            
        }

        private void xx(ElasticClient elastic)
        {
            var indexSettings = new IndexSettings();

            var customAnalyzer = new CustomAnalyzer();
            customAnalyzer.Tokenizer = "keyword";
            
            customAnalyzer.Filter = new[] { "lowercase" };
            indexSettings.Analysis.Analyzers.Add("custom_lowercase_analyzer", customAnalyzer);

            //var analyzerRes = elastic.CreateIndex("", ci => ci
            //   .Index("my_third_index")
            //   //.InitializeUsing(indexSettings)
            //   .AddMapping<TypeDescriptor>(m => m.MapFromAttributes())
            //   .AddMapping<MethodDescriptor>(m => m.MapFromAttributes()));
        }

        private Task<ICreateIndexResponse> CreateDocumentIndex(string indexName, ElasticClient elastic)
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

            return elastic.CreateIndexAsync(indexName, i => indexDescriptor);
        }

    }
}
