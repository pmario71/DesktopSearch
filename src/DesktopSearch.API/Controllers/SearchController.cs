using DesktopSearch.Core.DataModel.Documents;
using DesktopSearch.Core.ElasticSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.API.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        //[HttpPost]
        //public Task<IActionResult> Create([FromBody] DocDescriptor item)
        //{
        //    if (item == null)
        //    {
        //        return BadRequest();
        //    }

        //    await _searchService.IndexDocument(item);

        //    return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        //}
    }

    public class FileBatch
    {
        public string[] Files { get; set; }
    }
}
