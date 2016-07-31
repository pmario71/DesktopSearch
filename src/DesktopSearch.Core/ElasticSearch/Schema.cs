using DesktopSearch.Core.DataModel;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.ElasticSearch
{
    public class Schema
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

    }
}
