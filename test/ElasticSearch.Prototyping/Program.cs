using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch.Prototyping
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var test = new ElasticSearch_Indexing();
            //test.Index_CaseInsensitive();
            //test.Document();
            test.SearchOnly();

        }
    }
}
