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
        private UserDelegationKey _delegationKey;
         
        public BlobStore(BlobServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }
        
        public async Task<Uri> GetBlobReadUri(string category, string blobName, long atLeast, long noLonger)
        {
            var containerClient = _serviceClient.GetBlobContainerClient(category);
            await containerClient.CreateIfNotExistsAsync();
            
            var blobClient = containerClient.GetBlobClient(blobName);
            if (blobClient != null && blobClient.CanGenerateSasUri)
                return blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(atLeast));

            // Try to compose a SAS token by getting a delegate key
            if (_delegationKey == null || _delegationKey.SignedExpiresOn < DateTime.UtcNow)
                await UpdateDelegationKey();
            
            // Create a new Sas uri & return it.
            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = category,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddSeconds(atLeast)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                // Specify the user delegation key.
                Sas = sasBuilder.ToSasQueryParameters(_delegationKey, 
                    _serviceClient.AccountName)
            };

            return blobUriBuilder.ToUri();
        }

        public async Task WriteBlob(string category, string blobName, string mimeType, Stream contentStream)
        {
            var blobClient = _serviceClient.GetBlobContainerClient(category)?.GetBlobClient(blobName);
            if (blobClient == null) return;

            var headers = new BlobHttpHeaders
            {
                // always set caching to be fairly lenient. Clients can clear cache if it becomes an issue.
                CacheControl = "max-age=3600"
            };
            
            // conditionally set content type
            if (mimeType != null) headers.ContentType = mimeType;
            
            await blobClient.UploadAsync(contentStream, headers);
        }

        private async Task UpdateDelegationKey()
        {
            _delegationKey = await _serviceClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddDays(7));
        }
    }
}         