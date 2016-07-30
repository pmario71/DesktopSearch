using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeSearchTests.Indexing
{
    public class FileIOPerformanceTests
    {
        private static IEnumerable<DriveInfo> _drives;
        //private const string _filename = "d:\\tmp\\testdata.txt";
        private const string _filename = "{0}testdata.txt";

        const int _size = 20000000;

        //[TestFixtureSetUp]
        public void Setup()
        {
            _drives = System.IO.DriveInfo.GetDrives().Where(di => di.IsReady && di.DriveType == DriveType.Fixed);
        }

        //[TestFixtureTearDown]
        public static void Teardown()
        {
            foreach (var drive in _drives)
            {
                var file = GetFileName(drive);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        private static string GetFileName(DriveInfo drive)
        {
            string file = string.Format(_filename, drive.Name);
            return file;
        }

        [Fact(Skip="Not yet compatible with .net core!")]
        public void ReadFile_multiple_times_with_caching_enabled()
        {
            foreach (var drive in _drives)
            {
                Console.WriteLine("Tests using drive: {0}", drive);
                string filename = GetFileName(drive);

                var sw = Stopwatch.StartNew();
                createTestData(filename, _size);
                Console.WriteLine("create file: {0} ms", sw.ElapsedMilliseconds);

                for (int i = 0; i < 10; i++)
                {
                    sw = Stopwatch.StartNew();
                    File.ReadAllText(filename);
                    Console.WriteLine("{0,2}: {1} ms", i, sw.ElapsedMilliseconds);
                }
                Console.WriteLine("------------------------------------------");
            }
        }

        [Fact(Skip = "Not yet compatible with .net core!")]
        public void ReadFile_multiple_times_without_caching_enabled()
        {
            foreach (var drive in _drives)
            {
                Console.WriteLine("Tests using drive: {0}", drive);
                string filename = GetFileName(drive);

                var sw = Stopwatch.StartNew();
                createTestData(filename, _size);
                Console.WriteLine("create file: {0} ms", sw.ElapsedMilliseconds);

                for (int i = 0; i < 10; i++)
                {
                    FileSystemTools.FlushFSCacheForFile(filename);
                    sw = Stopwatch.StartNew();
                    File.ReadAllText(filename);
                    Console.WriteLine("{0,2}: {1} ms", i, sw.ElapsedMilliseconds);
                }
            }
        }

        private void createTestData(string path, int size)
        {
            var file = new FileInfo(path);
            if (file.Exists && file.Length == size)
            {
                return;
            }

            byte[] charArray = Encoding.UTF8.GetBytes("abcdefghij");
            using (var fs = new FileStream(path,FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                for (int i = 0; i < size/10; i++)
                {
                    fs.Write(charArray,0,10);
                }
            }
        }
    }

    class FileSystemTools
    {
        const int FILE_FLAG_NO_BUFFERING = 0x20000000;
        const FileOptions Unbuffered = (FileOptions)FILE_FLAG_NO_BUFFERING;

        /// <summary>
        /// Flush the file system cache for this file. This ensures that all subsequent operations on the file really cause disc
        /// access and you can measure the real disc access time.
        /// </summary>
        /// <param name="file">full path to file.</param>
        public static void FlushFSCacheForFile(string file)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.None | Unbuffered))
            {
            }
        }
    }
}
