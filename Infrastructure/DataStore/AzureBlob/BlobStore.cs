using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Constants;

namespace Viv2.API.Infrastructure.DataStore.AzureBlob
{
    public class BlobStore : IBlobStore
    {
        private readonly BlobServiceClient _serviceClient;
         
        public BlobStore(BlobServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }
        
        public async Task<Uri> GetBlobReadUri(string category, string blobName, long atLeast, long noLonger)
        {
            var blobClient = _serviceClient.GetBlobContainerClient(category)?.GetBlobClient(blobName);
            if (blobClient != null && blobClient.CanGenerateSasUri)
                return blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(atLeast));
            return null;
        }

        public async Task WriteBlob(string category, string blobName, string mimeType, Stream contentStream)
        {
            var blobClient = _serviceClient.GetBlobContainerClient(category)?.GetBlobClient(blobName);
            if (blobClient == null) return;
            
            var headers = (mimeType != null) ? new BlobHttpHeaders { ContentType = mimeType } : null;
            await blobClient.UploadAsync(contentStream, headers);
        }
    }
}         