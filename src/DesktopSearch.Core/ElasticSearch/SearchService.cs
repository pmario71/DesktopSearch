﻿using DesktopSearch.Core.Configuration;
using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Code;
using DesktopSearch.Core.DataModel.Documents;
using DesktopSearch.Core.Extractors.Roslyn;
using DesktopSearch.Core.Utils;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.ElasticSearch
{
    public class SearchService
    {
        private readonly ElasticSearchConfig _eleasticSearchConfig;
        private readonly ElasticClient       _elastic;
        private readonly RoslynParser        _roslynParser = new RoslynParser();

        #region CTOR
        public SearchService(ElasticSearchConfig elasticSearchConfig)
        {
            _eleasticSearchConfig = elasticSearchConfig;

            var settings = new ConnectionSettings(new Uri(_eleasticSearchConfig.Uri));
            var elastic = new ElasticClient(settings);

            var res = elastic.IndexExists(Configuration.CodeSearch.IndexName);
            if (!res.IsValid && !res.Exists)
            {
                EnsureIndicesCreated().Wait();
            }
            _elastic = elastic;
        }
        #endregion

        #region Initialization
        //private async Task<ElasticClient> InitializeAsync()
        //{
        //    var settings = new ConnectionSettings(new Uri(_eleasticSearchConfig.Uri));
        //    settings.DefaultIndex(Configuration.CodeSearch.IndexName);

        //    var elastic = new ElasticClient(settings);

        //    var res = await elastic.IndexExistsAsync(Configuration.CodeSearch.IndexName);

        //    if (!res.IsValid && !res.Exists)
        //    {
        //        await EnsureIndicesCreated();
        //    }

        //    return elastic;
        //}

        private async Task EnsureIndicesCreated()
        {
            // --------------------------------------------------------------------
            // setup index
            // --------------------------------------------------------------------
            Task codeIndexTask = null;
            Task docIndexTask = null;

            var elastic = _elastic;

            if (!elastic.IndexExists(Configuration.DocumentSearch.IndexName).Exists)
            {
                var docIndex = new CreateIndexDescriptor(Configuration.DocumentSearch.IndexName);

                docIndex.Mappings(mp => mp.Map<DocDescriptor>(m => m
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

                docIndexTask = elastic.CreateIndexAsync(Configuration.DocumentSearch.IndexName, i => docIndex);
            }

            if (!elastic.IndexExists(Configuration.CodeSearch.IndexName).Exists)
            {
                var indexDescriptor = new CreateIndexDescriptor(Configuration.CodeSearch.IndexName)
                    .Mappings(ms => ms
                        .Map<TypeDescriptor>(m => m.AutoMap())
                        .Map<MethodDescriptor>(m => m.AutoMap())
                        .Map<FieldDescriptor>(m => m.AutoMap()));

                codeIndexTask = elastic.CreateIndexAsync(Configuration.CodeSearch.IndexName, i => indexDescriptor);
            }

            await Task.WhenAll(docIndexTask, codeIndexTask);
        }
        #endregion

        #region API
        public async Task IndexDocument(string documentPath)
        {
            var docDesc = new DocDescriptor();
            docDesc.Path = documentPath;
            docDesc.Content = Convert.ToBase64String(File.ReadAllBytes(documentPath));

            var elastic = _elastic;
            var response = await elastic.IndexAsync(docDesc, s => s.Index(DocumentSearch.IndexName));

            if (!response.IsValid)
            {
                throw new Exception($"Failed to index document: '{documentPath}'", response.OriginalException);
            }
        }

        public async Task IndexCodeFile(string codefilePath)
        {
            using (StreamReader sr = new StreamReader(new FileStream(codefilePath, FileMode.Open, FileAccess.Read)))
            {
                String fileContent = await sr.ReadToEndAsync();
                var extractedTypes = _roslynParser.ExtractTypes(fileContent);

                var elastic = _elastic;
                var response = await elastic.IndexManyAsync(extractedTypes, CodeSearch.IndexName);

                if (!response.IsValid)
                {
                    throw new Exception($"Failed to index code file: '{codefilePath}'", response.OriginalException);
                }
            }
        }
        public async Task<DocDescriptor> GetDocumentAsync(string id)
        {
            var result = await _elastic.SearchAsync<DocDescriptor>(t => t.Index(DocumentSearch.IndexName).Query(q => q.Term(p => p.Path, id)));
            return result.Documents.First();
        }

        public async Task<IEnumerable<IHit<DocDescriptor>>> SearchDocumentAsync(string querystring)
        {
            var result = await _elastic.SearchAsync<DocDescriptor>(t => t.Index(DocumentSearch.IndexName).Query(q => q.QueryString(c => c.Query(querystring))));
            return result.Hits;
        }
        #endregion

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
