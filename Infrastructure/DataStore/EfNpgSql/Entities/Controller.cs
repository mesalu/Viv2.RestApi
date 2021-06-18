using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    /// <summary>
    /// Concrete entity implementation for IController, used in EfCore implementations.
    /// </summary>
    public class Controller : IController
    {
        public Controller()
        {
            BackedEnvironments = new HashSet<Environment>();
        }
        
        // Properties visible to EF that define relationships via concrete classes.
        public Guid Id { get; set; }
        public ICollection<Environment> BackedEnvironments { get; set; }
        public User RealOwner { get; set; }
        
        // Abstraction copmliance.
        [NotMapped]
        public ICollection<IEnvironment> Environments => 
            BackedEnvironments.Select(be => be as IEnvironment).ToList();
        [NotMapped] public IUser Owner => RealOwner;
    }
}