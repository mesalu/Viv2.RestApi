using System;
using System.Collections.Generic;

#nullable enable

namespace Viv2.API.Core.ProtoEntities
{
    public interface IEnvironment
    {
        public Guid? Id { get; set; }
        public IController? Controller { get; set; }
        public IPet? Inhabitant { get; set; }
        public string? Model { get; set; }
        public string? Descr { get; set; }

        public ICollection<IEnvDataSample> EnvDataSamples { get; }
        public ICollection<IUser> Users { get; }
    }
}
