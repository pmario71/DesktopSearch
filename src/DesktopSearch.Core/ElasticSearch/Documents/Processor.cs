using DesktopSearch.Core.DataModel.Documents;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.ElasticSearch.Documents
{
    /// <summary>
    /// 
    /// </summary>
    public class Processor
    {
        private ElasticClient _client;

        public Processor(ElasticClient client)
        {
            _client = client;
        }

        public async Task ProcessFile(string filePath)
        {
            var content = Convert.ToBase64String(File.ReadAllBytes(filePath));
            var doc = new DocDescriptor
            {
                Path = filePath,
                Content = content
            };

            var result = await _client.IndexAsync<DocDescriptor>(doc);

            if (!result.IsValid)
            {
                throw new Exception($"Failed to process document: {filePath}!", result.OriginalException);
            }
        }

    }
}
