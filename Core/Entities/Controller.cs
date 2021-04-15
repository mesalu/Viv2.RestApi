using System;
using System.Collections.Generic;

namespace Viv2.API.Core.Entities
{
    public class Controller
    {
        public Controller()
        {
            Environments = new HashSet<Environment>();
        }

        public Guid Id { get; set; }

        public virtual ICollection<Environment> Environments { get; set; }
    }
}