using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class Environment : IEnvironment
    {
        public Environment()
        {
            Samples = new HashSet<EnvDataSample>();
            BackedUsers = new HashSet<User>();
        }
        
        public Guid? Id { get; set; }
        public IController? Controller { get; set; }
        
        // what EF needs
        public Pet? RealInhabitant { get; set; }
        
        // What Core needs
        [NotMapped]
        public IPet? Inhabitant 
        { 
            get => RealInhabitant;
            set
            {
                if (value == null) RealInhabitant = null;
                else
                {
                    if (value is Pet pet) RealInhabitant = pet;
                    else throw new ArgumentException("Mismatched infrastructure datastore implementations");
                }
            } 
        }
        
        public string? Model { get; set; }
        public string? Descr { get; set; }
        
        public ICollection<EnvDataSample> Samples { get; set; }
        
        // For abstraction compliance (what Core accesses)
        [NotMapped]
        public ICollection<IEnvDataSample> EnvDataSamples => Samples.Select(s => s as IEnvDataSample).ToList();
        
        // For interfacing with entities (What EF needs)
        public virtual ICollection<User> BackedUsers { get; set; }
        
        // For abstraction compliance (what Core accesses)
        [NotMapped] 
        public ICollection<IUser> Users => BackedUsers.Select(bu => bu as IUser).ToList();

    }
}