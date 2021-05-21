using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Viv2.API.AppInterface.Constants;
using Viv2.API.AppInterface.Ports;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.AppInterface.Controllers
{
    [ApiController]
    [Route(RoutingStrings.BaseController)]
    public class BlobController : ControllerBase
    {
        private readonly IBlobAccessUseCase _useCase;
        
        public BlobController(IBlobAccessUseCase useCase)
        {
            _useCase = useCase;
        }
        
        [HttpPut]
        public async Task<IActionResult> PutBlob(string blobName)
        {
            var request = new BlobStorageRequest
            {
                Mode = BlobStorageRequest.Operation.Write,
                Category = "images",
                BlobName = blobName,
                UserId = Guid.NewGuid(),
                Content = Request.Body
            };
            
            var port = new BasicPresenter<BlobUriResponse>();
            var success = await _useCase.Handle(request, port);
            
            if (!success) return BadRequest("API error");
            return new OkObjectResult(port.Response)
            {
                StatusCode = 201 // CreatedAtResult wasn't quite my cup of tea, should figure that out.
            };
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBlobUri(string blobName)
        {
            
            var request = new BlobStorageRequest
            {
                Mode = BlobStorageRequest.Operation.Read,
                Category = "images",
                BlobName = blobName,
                UserId = Guid.NewGuid()
            };
            var port = new BasicPresenter<BlobUriResponse>();

            var success = await _useCase.Handle(request, port);
            return (success) ? new OkObjectResult(port.Response) : new NotFoundResult();
        }
    }
}