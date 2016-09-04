using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Configuration
{
    public class Settings
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        private Uri _elasticSearchUri;

        [JsonIgnore]
        public Uri ElasticSearchUri
        {
            get
            {
                if (_elasticSearchUri == null)
                {
                    return new Uri("http://localhost:9200");
                }
                return _elasticSearchUri;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("Null value not cannot be specified as ElasticSearchUri!");
                if (value.Port <= 0)
                    throw new ArgumentException($"No valid port specified: {value}!");

                _elasticSearchUri = value;
            }
        }

        public FoldersToIndex FoldersToIndex { get; set; }
    }

    public class FoldersToIndex
    {
        public Folder[] Folders { get; set; }
    }

    public class Folder
    {
        public string Path { get; set; }

        /// <summary>
        /// Defines which type of indexing to apply to the folder.
        /// </summary>
        public string IndexingType { get; set; }
    }

    public class IndexingType
    {
        public string Name { get; set; }

        public string[] ExcludedExtensions { get; set; }

        public string[] IncludedExtensions { get; set; }
    }
}
