using System;
using System.IO;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class BlobStorageRequest : IUseCaseRequest<BlobUriResponse>
    {
        public enum Operation { Read, Write }
        public Operation Mode { get; set; }
        public Guid UserId { get; set; } 
        public string Category { get; set; }
        
        /// <summary>
        /// The name of the blob to interact with, if null and write mode is requested
        /// then a new blob name is generated.
        /// </summary>
        public string BlobName { get; set; }
        
        /// <summary>
        /// The body / content stream for use when writing a blob.
        /// </summary>
        public Stream Content { get; set; }
    }
}