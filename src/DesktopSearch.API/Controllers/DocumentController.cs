using DesktopSearch.Core.ElasticSearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.API.Controllers
{
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

            try
            {
                _searchService.IndexDocument(filepath).Wait();
            }
            catch(Exception ex)
            {
                int i = 0;
            }

            return new CreatedAtRouteResult(routeName: "GetID", routeValues: filepath, value: "it worked");
            //return CreatedAtRoute("GetDoc", new { id = filepath });
        }

        //POST /api/document HTTP/1.1
        //Host: localhost:5000
        //Content-Type: application/json
        //Cache-Control: no-cache
        //Postman-Token: 0db8e493-57d6-92ac-5dbc-5fb500196306

        //["D:\\Todo\\banking.txt", "D:\\Todo\\LaskerDockerDotNet.potx"]

        [HttpPost]
        public async Task<IActionResult> CreateMany([FromBody]string[] filepaths)
        {
            if (filepaths == null)
            {
                return BadRequest("Failed to deserialize body. Example: [\"D:\\Todo\\banking.txt\", \"D:\\Todo\\LaskerDockerDotNet.potx\"]");
            }

            var errors = new List<string>();
            foreach (var filepath in filepaths)
            {
                if (!System.IO.File.Exists(filepath))
                {
                    errors.Add(filepath);
                }
                else
                {
                    await _searchService.IndexDocument(filepath);
                }
            }
            if (errors.Any())
            {
                var sb = new StringBuilder();
                sb.AppendLine("Failed to index following files:");
                foreach (var item in errors)
                {
                    sb.AppendLine($"  - {item}");
                }

                _log.LogWarning(sb.ToString());
            }

            return new CreatedAtRouteResult(routeName: "GetIDs", routeValues: "ddddd", value: "it worked");
            //return CreatedAtRoute("GetDoc", new { id = filepath });
        }

        [HttpGet("{id}", Name = "GetID")]
        public async Task<string> GetDocumentByID(string id)
        {
            var res = await _searchService.GetDocumentAsync(id);
            return res.Path;
        }

        [HttpGet("{query}", Name = "Search")]
        public async Task<string> Search(string query)
        {
            var results = await _searchService.SearchDocumentAsync(query);

            var sb = new StringBuilder();

            foreach (var item in results)
            {
                sb.AppendLine($"{item.Score}\t{item.Source.Path}");
            }

            return sb.ToString();
        }
        //default_field
    }
}
