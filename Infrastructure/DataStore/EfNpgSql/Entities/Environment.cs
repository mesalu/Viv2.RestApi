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
     
        // Properties discoverable by EF Core that utilize concrete classes to define relations.
        public Guid? Id { get; set; }
        public Controller? RealController { get; set; }
        public Pet? RealInhabitant { get; set; }
        public string? Model { get; set; }
        public string? Descr { get; set; }
        public ICollection<EnvDataSample> Samples { get; set; }
        public virtual ICollection<User> BackedUsers { get; set; }

        // All the hidden abstraction implementations (fulfillment of the Core interface that don't play
        // too nicely with EF Core.) 
        [NotMapped]
        public IController? Controller
        {
            get => RealController;
            set
            {
                if (value is Controller controller) RealController = controller;
                // else throw exception?
            }
        }
        
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
        
        [NotMapped]
        public ICollection<IEnvDataSample> EnvDataSamples => Samples.Select(s => s as IEnvDataSample).ToList();
        
        // For abstraction compliance (what Core accesses)
        [NotMapped] 
        public ICollection<IUser> Users => BackedUsers.Select(bu => bu as IUser).ToList();

    }
}