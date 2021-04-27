using System;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class NewSampleRequest : IUseCaseRequest<BlankResponse>
    {
        public Guid UserId { get; set; }
        public EnvSampleSubmission Sample { get; set; }
    }
}