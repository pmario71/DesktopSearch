using DesktopSearch.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DesktopSearch.Core.Tests.FileSystem
{
    public class ExcludeFileByExtensionFilterTests
    {

        [Theory]
        [InlineData(new string[] { }, new[] { "c:\\temp\\Test.cs", "c:\\test.xaml" })]
        [InlineData(new[] { "c:\\temp\\Test.txt" }, new[] { "c:\\temp\\Test.txt", "c:\\test.xaml" })]
        public void FilterByExtension_over_enumeration(string[] expectedResult, string[] file)
        {
            var sut = new ExcludeFileByExtensionFilter("cs", "xaml");
            var result = sut.FilterByExtension(file);
            
            Assert.True(expectedResult.SequenceEqual(result));
        }

        [Theory]
        [InlineData("c:\\temp\\Test.cs", false)]
        [InlineData("c:\\temp\\Test.1.cs", false)]
        [InlineData("c:\\temp\\Test.1.CS", false)]
        [InlineData("c:\\temp\\Test.1.Cs", false)]
        [InlineData("c:\\temp\\Test.1.txt", true)]
        [InlineData("c:\\temp\\Test.xaml", false)]
        public void FilterByExtension_over_single_value(string file, bool expectedResult)
        {
            var sut = new ExcludeFileByExtensionFilter("cs", "xaml");
            Assert.Equal(expectedResult, sut.FilterByExtension(file));
        }
    }
}
