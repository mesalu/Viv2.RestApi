using System;
using System.Collections.Generic;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.AppInterface.Dto
{
    public class PeerUpdateForm
    {
        public IEnumerable<Guid> PeerIds { get; set; }
        public Guid ControllerId { get; set; }
    }
}