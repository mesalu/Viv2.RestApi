using System;
using System.Threading.Tasks;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.Core.UseCases
{
    public class BlobAccessUseCase : IBlobAccessUseCase
    {
        private readonly IBlobStore _blobStore;

        public BlobAccessUseCase(IBlobStore blobStore)
        {
            _blobStore = blobStore;
        }
        public async Task<bool> Handle(BlobStorageRequest message, IOutboundPort<BlobUriResponse> outputPort)
        {
            // Trust user guid? Ideally only authenticated controllers are getting here.
            if (message.UserId == Guid.Empty) return false;
            
            // compose blob name if necessary
            if (string.IsNullOrWhiteSpace(message.BlobName) && message.Mode == BlobStorageRequest.Operation.Write)
                message.BlobName = $"{message.UserId}_{DateTime.UtcNow}_{Guid.NewGuid()}";
            
            //  can't read a non-existent blob.
            if (string.IsNullOrWhiteSpace(message.BlobName)) return false;

            if (message.Mode == BlobStorageRequest.Operation.Write)
            {
                if (message.Content == null) return false; // can't write with no content.
                await _blobStore.WriteBlob(message.Category, message.BlobName, null,  message.Content);
            }
            
            var uri = await _blobStore.GetBlobReadUri(message.Category, message.BlobName, 60, 60);
            
            // call through to blob store method.
            var response = new BlobUriResponse
            {
                Uri = uri,
                ExpiresIn = 60
            };
           
            // send back response
            outputPort.Handle(response);
            return true;
        }
    }
}