using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class Controller : IController
    {
        public Controller()
        {
            BackedEnvironments = new HashSet<Environment>();
        }
        
        public Guid Id { get; set; }
        
        // what EF needs
        public ICollection<Environment> BackedEnvironments { get; set; }
        
        // for abstraction
        [NotMapped]
        public ICollection<IEnvironment> Environments => 
            BackedEnvironments.Select(be => be as IEnvironment).ToList();
    }
}