﻿using DesktopSearch.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DesktopSearch.PS.Tests.Configuration
{
    public class ConfigAccessTests
    {

        [Fact]
        public void NullValueHandling_for_ElasticSearchURI_Test()
        {
            Settings settings = new Settings
            {
                FoldersToIndex = new FoldersToIndex
                {
                    Folders = new[]
                    {
                        new Folder{ Path = "d:\\temp", IndexingType="Code" }
                    }
                }
            };

            var strm = new MemoryStreamEx();
            var sut = new ConfigAccess(new TestFactory(strm));

            sut.SaveChanges(settings);

            strm.Position = 0;

            var sr = new StreamReader(strm);
            var s = sr.ReadToEnd();

            // check that stream does not contain serialized 
            Assert.DoesNotContain(s, "localhost");

            strm.Position = 0;

            var result = sut.Get();

            Assert.Equal(settings.FoldersToIndex.Folders[0].Path, result.FoldersToIndex.Folders[0].Path);

            //var c = new KellermanSoftware.CompareNetObjects.CompareLogic();
            //var compareResult = c.Compare(settings, result);

            //if (!compareResult.AreEqual)
            //{
            //    Trace.TraceInformation(compareResult.DifferencesString);
            //}

            //Assert.True(compareResult.AreEqual, compareResult.DifferencesString);
        }

        [Fact]
        public void SerializeDeserializeTest()
        {
            Settings settings = new Settings
            {
                ElasticSearchUri = new Uri("http://test.com:1234"),
                FoldersToIndex = new FoldersToIndex
                {
                    Folders = new[]
                    {
                        new Folder{ Path = "d:\\temp", IndexingType="Code" }
                    }
                }
            };

            var strm = new MemoryStreamEx();
            var sut = new ConfigAccess(new TestFactory(strm));

            sut.SaveChanges(settings);

            strm.Position = 0;

            var sr = new StreamReader(strm);
            var s = sr.ReadToEnd();

            strm.Position = 0;

            var result = sut.Get();

            Assert.Equal(settings.FoldersToIndex.Folders[0].Path, result.FoldersToIndex.Folders[0].Path);

            //var c = new KellermanSoftware.CompareNetObjects.CompareLogic();
            //var compareResult = c.Compare(settings, result);

            //if (!compareResult.AreEqual)
            //{
            //    Trace.TraceInformation(compareResult.DifferencesString);
            //}

            //Assert.True(compareResult.AreEqual, compareResult.DifferencesString);
        }

    }

    internal class TestFactory : IStreamFactory
    {
        private MemoryStream strm;

        public TestFactory(MemoryStream strm)
        {
            this.strm = strm;
        }

        public Stream GetReadableStream()
        {
            return strm;
        }

        public Stream GetWritableStream()
        {
            return strm;
        }
    }
}
