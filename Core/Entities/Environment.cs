using System;
using System.Collections.Generic;

#nullable enable

namespace Viv2.API.Core.Entities
{
    public class Environment
    {
        public Environment()
        {
            EnvDataSamples = new HashSet<EnvDataSample>();
            Users = new HashSet<User>();
        }

        public Guid? Id { get; set; }
        public Controller? Controller { get; set; }
        public Pet? Inhabitant { get; set; }
        public string? Model { get; set; }
        public string? Descr { get; set; }

        public virtual ICollection<EnvDataSample> EnvDataSamples { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
