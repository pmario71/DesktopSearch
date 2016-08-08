using DesktopSearch.Core.DataModel.Documents;
using DesktopSearch.Core.ElasticSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly ILogger _log;
        private readonly SearchService _searchService;

        public DocumentController(SearchService searchService, ILoggerFactory log)
        {
            _searchService = searchService;
            _log = log.CreateLogger("DocumentController");
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(string filepath)
        {
            if (!System.IO.File.Exists(filepath))
            {
                var msg = $"File does not exist or is not accessible: '{filepath}'";
                _log.LogWarning(msg);
                return base.BadRequest(msg);
            }
            _log.LogInformation($"Created item for filepath: {filepath}");
            _searchService.IndexDocument(filepath).Wait();

            return new CreatedAtRouteResult(routeName: "GetID", routeValues: filepath, value: "it worked");
            //return CreatedAtRoute("GetDoc", new { id = filepath });
        }

        [HttpGet("{id}", Name = "GetID")]
        public string GetID(int id)
        {
            return $"Id: {id}";
        }
    }
}
