using System;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class MigratePetRequest : IUseCaseRequest<BlankResponse>
    {
        public Guid UserId { get; set; }
        public Guid EnvId { get; set; }
        public int PetId { get; set; }
    }
}