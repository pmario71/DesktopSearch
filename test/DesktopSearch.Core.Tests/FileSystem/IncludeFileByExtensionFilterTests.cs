using DesktopSearch.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DesktopSearch.Core.FileSystem
{
    public class IncludeFileByExtensionFilterTests
    {

        [Theory]
        [InlineData("c:\\temp\\Test.cs")]
        [InlineData("c:\\temp\\Test.1.cs")]
        [InlineData("c:\\temp\\Test.1.CS")]
        [InlineData("c:\\temp\\Test.1.Cs")]
        public void FilterByExtension_over_enumeration(string file)
        {
            var files = new[] {file};

            var sut = new IncludeFileByExtensionFilter("cs");
            var result = sut.FilterByExtension(files);
            
            //CollectionAssert.Equal(files, result, StringComparer.OrdinalIgnoreCase);
            Assert.True(files.SequenceEqual(result));
        }

        [Theory]
        [InlineData("c:\\temp\\Test.cs", true)]
        [InlineData("Test.cs", true)]
        [InlineData("c:\\temp\\Test.1.cs", true)]
        [InlineData("c:\\temp\\Test.1.Cs", true)]
        [InlineData("Test.fds", false)]
        public void FilterByExtension_over_single_value(string file, bool expectedResult)
        {
            var sut = new IncludeFileByExtensionFilter("cs");
            Assert.Equal(expectedResult, sut.FilterByExtension(file));
        }
    }
}
