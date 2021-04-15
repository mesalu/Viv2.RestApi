using System;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class ProviderGrantRequest : IUseCaseRequest<LoginResponse>
    {
        public Guid OnBehalfOf { get; set; }
    }
}
