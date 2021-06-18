using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Viv2.API.Core.ProtoEntities;

#nullable enable

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class Pet : IPet
    {
        private EnvDataSample? _latestConcreteSample;
        
        public Pet() {}
        
        public Pet(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        
        private ILazyLoader LazyLoader;

        // All the properties visible to EF Core:
        public int Id { get; set; }
        public string Name { get; set; }
        public string Morph { get; set; }
        public DateTime? HatchDate { get; set; }
        public User RealCareTaker { get; set; }
        public Species RealSpecies { get; set; }
        public Environment RealEnclosure { get; set; }
        public BlobRecord ProfileRecordEntity { get; set; }
        public EnvDataSample? LatestConcreteSample
        {
            get => LazyLoader?.Load(this, ref _latestConcreteSample);
            set => _latestConcreteSample = value;
        }
        
        // Abstraction compliance, fulfill the Core interface.
        [NotMapped]
        public ISpecies Species 
        { 
            get => RealSpecies;
            set
            {
                if (value is Species species) RealSpecies = species;
            } 
        }
        [NotMapped] public IUser CareTaker => RealCareTaker;
        [NotMapped] public IEnvironment? Enclosure => RealEnclosure;
        [NotMapped] public IBlobRecord? ProfileImage => ProfileRecordEntity;

        // TODO: maintain a many-to-many with past profile pictures?
        //  may be worthwhile even if just for record keeping.
        
        [NotMapped] public IEnvDataSample? LatestSample => LatestConcreteSample;
    }
}