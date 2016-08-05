using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics.Contracts;
using System.IO;
using DesktopSearch.Core.DataModel;
using DesktopSearch.Core.DataModel.Code;

namespace DesktopSearch.Core.Extractors
{
    #region IExtractor contract binding

    /// <summary>
    /// Interface that defines the contract for all Extractors that can be used interchangeably.
    /// </summary>
    //[ContractClass(typeof(IExtractorContract))]
    public interface IExtractor
    {
        /// <summary>
        /// Parses a file and extracts all <see cref="TypeDescriptor"/>s that are contained.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath">file to be indexed</param>
        /// <returns>Lucene <see cref="Document"/></returns>
        IEnumerable<TypeDescriptor> Extract(ParserContext context, FileInfo filePath);
    }

    //[ContractClassFor(typeof(IExtractor))]
    //// ReSharper disable once InconsistentNaming
    //abstract class IExtractorContract : IExtractor
    //{
    //    public IEnumerable<TypeDescriptor> Extract(ParserContext context, FileInfo filePath)
    //    {
    //        Contract.Requires<ArgumentNullException>(context != null);
    //        Contract.Requires<ArgumentNullException>(filePath != null);

    //        return default(IEnumerable<TypeDescriptor>);
    //    }
    //}
    #endregion

}