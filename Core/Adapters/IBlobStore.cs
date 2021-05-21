using System;
using System.Threading.Tasks;
using System.IO;

namespace Viv2.API.Core.Adapters
{
    /// <summary>
    /// Manages interfacing with large binary object storage.
    ///
    /// Rather than offering streams / large loads it manages
    /// composing URIs & manages access to blob storage.
    /// </summary>
    public interface IBlobStore
    {
        /// <summary>
        /// Composes & returns a URI for accessing the blob specified by category & blobName.
        /// </summary>
        /// <param name="category">storage category, e.g. 'images'</param>
        /// <param name="blobName">blob name</param>
        /// <param name="atLeast">a minimum bound on how long (in seconds) the given URI should be valid for.</param>
        /// <param name="noLonger">a maximum bound on how long (in seconds) the given URI should be valid for.</param>
        /// <returns></returns>
        Task<Uri> GetBlobReadUri(string category, string blobName, long atLeast, long noLonger);

        /// <summary>
        /// Commissions a URI for storing a blob.
        /// </summary>
        /// <param name="category">storage category, e.g. 'iamges'</param>
        /// <param name="blobName">blob name, should be specific to user & context</param>
        /// <param name="contentType">The Mime type of the blob</param>
        /// <param name="contentStream"></param>
        /// <returns></returns>
        Task WriteBlob(string category, string blobName, string contentType, Stream contentStream);
    }
}