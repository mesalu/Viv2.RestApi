using System;

namespace Viv2.API.Core.Dto.Response
{
    public class BlobUriResponse
    {
        public Uri Uri { get; set; }
        public long ExpiresIn { get; set; }
    }
}