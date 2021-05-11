using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class User : IdentityUser, IUser
    {
        public User() : base()
        {
            BackedPets = new HashSet<Pet>();
            BackedEnvironments = new HashSet<Environment>();
            BackedControllers = new HashSet<Controller>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public string Name
        {
            get => UserName;
            set => UserName = value;
        }

        [NotMapped]
        public Guid Guid 
        {
            get => (Id != null) ? Guid.Parse(Id) : Guid.Empty;
            set
            {
                if (value != Guid.Empty) Id = value.ToString();
            } 
        }

        // For actual mapping to entities.
        public virtual ICollection<Pet> BackedPets { get; set; }
        
        public virtual ICollection<Environment> BackedEnvironments { get; set; }
        
        public virtual ICollection<Controller> BackedControllers { get; set; }
        
        public virtual ICollection<RefreshToken> RefreshTokens { get; }
        
        // For abstraction compliance.
        [NotMapped]
        public ICollection<IPet> Pets => BackedPets.Select(p => p as IPet).ToList();

        [NotMapped]
        public ICollection<IEnvironment> Environments =>
            BackedEnvironments.Select(e => e as IEnvironment).ToList();

        [NotMapped]
        public ICollection<IController> Controllers =>
            BackedControllers.Select(c => c as IController).ToList();
    }
}
