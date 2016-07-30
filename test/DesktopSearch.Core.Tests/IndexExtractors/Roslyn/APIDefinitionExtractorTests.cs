/* -------------------------------------------------------------------------------------------------
   Restricted - Copyright (C) Siemens Healthcare GmbH/Siemens Medical Solutions USA, Inc., 2016. All rights reserved
   ------------------------------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSearch.Extractors.Roslyn;
using Xunit;

namespace CodeSearchTests.Indexing.Roslyn
{
    public class APIDefinitionExtractorTests
    {

        [Theory]
        [InlineData("API:NO", API.No)]
        [InlineData("API :  NO", API.No)]
        [InlineData("API:YES", API.Yes)]
        [InlineData("API :  YES", API.Yes)]
        [InlineData("Some other text API test.", API.Undefined)]
        [InlineData("Some other text API: test.", API.Undefined)]
        public void ParseTests(string comment, API expectedResult)
        {
            Assert.Equal(expectedResult, APIDefinitionExtractor.Parse(comment));
        }

    }
}
