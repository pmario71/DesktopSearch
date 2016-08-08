using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesktopSearch.Core.Configuration
{
    public class ElasticSearchConfig
    {
        private string _uri = "http://localhost:9200";

        public string Uri
        {
            get
            {
                return _uri;
            }

            set
            {
                _uri = value;
            }
        }
    }
}
