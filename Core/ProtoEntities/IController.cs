using System;
using System.Collections.Generic;

namespace Viv2.API.Core.ProtoEntities
{
    public interface IController
    {
        public Guid Id { get; set; }

        public ICollection<IEnvironment> Environments { get; }
    }
}