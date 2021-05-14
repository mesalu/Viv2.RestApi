using System;
using System.Collections;
using System.Collections.Generic;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Dto.Request
{
    public class NodeUpdateRequest : IUseCaseRequest<GenericDataResponse<IController>>
    {
        public enum Mode { Create, PeerUpdate }
        public Guid UserId { get; set; }
        public Guid ControllerId { get; set; }
        public IEnumerable<Guid> PeerIds { get; set; }
        public Mode Operation { get; set; }
    }
}