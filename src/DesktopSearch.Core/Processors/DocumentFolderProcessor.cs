using DesktopSearch.Core.Configuration;
using DesktopSearch.Core.DataModel.Documents;
using DesktopSearch.Core.FileSystem;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DesktopSearch.Core.Processors
{
    public class DocumentFolderProcessor : IFolderProcessor
    {
        private IElasticClient _client;
        private ILogger<DocumentFolderProcessor> _logging;

        public DocumentFolderProcessor(IElasticClient client)//, ILogger<DocumentFolderProcessor> logging)
        {
            _client = client;
            //_logging = logging;
        }

        public Task Process(Folder folder)
        {
            return Process(folder, null);
        }

        public Task Process(Folder folder, IProgress<int> progress)
        {
            var extensionFilter = new ExcludeFileByExtensionFilter(".bin", ".lnk");

            var filesToProcess = Directory.GetFiles(folder.Path, "*", SearchOption.AllDirectories)
                                          .Where(f => extensionFilter.FilterByExtension(f));

            return ExtractFilesAsync(filesToProcess, progress);
        }

        private async Task ExtractFilesAsync(IEnumerable<string> filesToParse, IProgress<int> progress)
        {
            int current = 0;
            var maxFiles = filesToParse.Count();

            foreach (var filePath in filesToParse)
            {
                var stopWatch = Stopwatch.StartNew();
                var content = Convert.ToBase64String(File.ReadAllBytes(filePath));
                var doc = new DocDescriptor
                {
                    Path = filePath,
                    Content = content
                };

                var result = await _client.IndexAsync<DocDescriptor>(doc);

                if (!result.IsValid)
                {
                    _logging.LogWarning($"Failed to index document: {filePath}!", result.OriginalException);
                }
                else
                {
                    stopWatch.Stop();
                    //_logging.LogInformation($"Adding '{Path.GetFileName(filePath)}' to index  took: {stopWatch.Elapsed.TotalSeconds} [s]");
                }

                ++current;
                if (progress != null)
                {
                    progress.Report((int)(current * 100 / (double)maxFiles));
                }
            }
        }
    }
}
