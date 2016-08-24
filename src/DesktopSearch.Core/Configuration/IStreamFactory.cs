using System;
using System.IO;

namespace DesktopSearch.Core.Configuration
{
    /// <summary>
    /// Extension interface to allow testing <see cref="ConfigAccess"/> without filesystem.
    /// </summary>
    public interface IStreamFactory
    {
        Stream GetWritableStream();
        Stream GetReadableStream();
    }

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