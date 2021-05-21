using System;
using System.IO;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class PetImageRequest : IUseCaseRequest<BlobUriResponse>
    {
        public bool Update { get; set; }
        public int PetId { get; set; }
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Content stream used when uploading an image.
        /// </summary>
        public Stream Content { get; set; }
        
        /// <summary>
        /// Used when creating blob, specifies the content type of the blob.
        /// </summary>
        public string MimeType { get; set; }
    }
}