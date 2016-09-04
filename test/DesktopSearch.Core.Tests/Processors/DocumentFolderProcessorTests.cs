using DesktopSearch.Core.DataModel.Documents;
using DesktopSearch.Core.Processors;
using Microsoft.Extensions.Logging;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DesktopSearch.Core.Tests.Processors
{
    public class DocumentFolderProcessorTests : IDisposable
    {
        private List<string> _foldersCreated = new List<string>();

        public void Dispose()
        {
            foreach (var folder in _foldersCreated)
            {
                Directory.Delete(folder, true);
            }
        }


        [Fact]
        public void Run_on_empty_folder()
        {
            var client = new Moq.Mock<IElasticClient>();
            var logging = new Moq.Mock<ILogger<DocumentFolderProcessor>>();

            string folder = CreateTestFolder();


            var sut = new DocumentFolderProcessor(client.Object/*, logging.Object*/);

            Configuration.Folder cfgFolder = new Configuration.Folder { IndexingType ="", Path=folder };
            sut.Process(cfgFolder);
        }

        [Fact]
        public async void Run_on_file()
        {
            var client = new Moq.Mock<IElasticClient>();
            var logging = new Moq.Mock<ILogger<DocumentFolderProcessor>>();

            string folder = CreateTestFolder();
            var filePath = folder + "\\TestDocument.txt";
            File.WriteAllText(filePath, "The content");

            // ensure that mock return
            var result = new Mock<IIndexResponse>();
            result.Setup(t => t.IsValid)
                .Returns(true);

            client.Setup(t => t.IndexAsync<DocDescriptor>(It.Is<DocDescriptor>(d => d.Path == filePath), null))
                .Returns(Task.FromResult(result.Object));


            var sut = new DocumentFolderProcessor(client.Object/*, logging.Object*/);

            Configuration.Folder cfgFolder = new Configuration.Folder { IndexingType = "", Path = folder };
            await sut.Process(cfgFolder);

            client.VerifyAll();            
        }

        private string CreateTestFolder()
        {
            var newFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(newFolder);
            _foldersCreated.Add(newFolder);

            return newFolder;
        }
    }
}
