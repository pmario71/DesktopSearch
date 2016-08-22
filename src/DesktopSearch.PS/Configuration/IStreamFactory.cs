using System;
using System.ComponentModel.Composition;
using System.IO;

namespace DesktopSearch.PS.Configuration
{
    public interface IStreamFactory
    {
        Stream GetWritableStream();
        Stream GetReadableStream();
    }

    [Export(typeof(IStreamFactory))]
    public class FileStreamFactory : IStreamFactory
    {
        private const string _settingsFile = "appsettings.json";
        private readonly string _directory;

        public FileStreamFactory()
        {
            _directory = Directory.GetCurrentDirectory();
        }

        public Stream GetReadableStream()
        {
            return new FileStream(Path.Combine(_directory, _settingsFile), FileMode.Open, FileAccess.Read);
        }

        public Stream GetWritableStream()
        {
            return new FileStream(Path.Combine(_directory, _settingsFile), FileMode.Truncate, FileAccess.Write);
        }
    }

    public class MemoryStreamEx : MemoryStream
    {
        /// <summary>
        /// Do not dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
        }
    }
}