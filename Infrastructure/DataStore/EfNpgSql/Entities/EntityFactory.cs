using System;
using System.Collections.Generic;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Core.Services;

#nullable enable

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities
{
    public class EntityFactory : IEntityFactory
    {
        private sealed class UserBuilder : IEntityFactory.IUserBuilder
        {
            private readonly User _user;

            public UserBuilder()
            {
                _user = new User();
            }
            
            public IEntityFactory.IUserBuilder AddName(string name)
            {
                _user.Name = name;
                return this;
            }

            public IEntityFactory.IUserBuilder AddEmail(string email)
            {
                _user.Email = email;
                return this;
            }
            
            public IUser Build()
            {
                return new User
                {
                    Name = _user.Name,
                    Email = _user.Email
                };
            }
        }
        private sealed class EnvBuilder : IEntityFactory.IEnvBuilder
        {
            private readonly Environment _env;

            public EnvBuilder()
            {
                _env = new Environment();
            }
            
            public IEntityFactory.IEnvBuilder SetId(Guid id)
            {
                _env.Id = id;
                return this;
            }

            public IEntityFactory.IEnvBuilder SetInhabitant(IPet? pet)
            {
                _env.Inhabitant = pet;
                return this;
            }

            public IEntityFactory.IEnvBuilder SetModelInfo(string model)
            {
                _env.Model = model;
                return this;
            }

            public IEntityFactory.IEnvBuilder SetDescription(string descr)
            {
                _env.Descr = descr;
                return this;
            }

            public IEntityFactory.IEnvBuilder WithSamples(IEnumerable<IEnvDataSample> samples)
            {
                foreach (var sample in samples)
                {
                    _env.EnvDataSamples.Add(sample);
                }

                return this;
            }

            public IEntityFactory.IEnvBuilder AddSample(IEnvDataSample sample)
            {
                _env.EnvDataSamples.Add(sample);
                return this;
            }

            public IEnvironment Build()
            {
                return new Environment
                {
                    Id = _env.Id,
                    Controller = _env.Controller,
                    Descr = _env.Descr,
                    Inhabitant =  _env.Inhabitant,
                    Samples = _env.Samples, // TODO: clone?
                };
            }
        }
        private sealed class PetBuilder : IEntityFactory.IPetBuilder
        {
            private readonly Pet _pet;

            public PetBuilder()
            {
                _pet = new Pet();
            }
            
            public IEntityFactory.IPetBuilder SetName(string name)
            {
                _pet.Name = name;
                return this;
            }

            public IEntityFactory.IPetBuilder SetSpecies(ISpecies species)
            {
                _pet.Species = species;
                return this;
            }

            public IEntityFactory.IPetBuilder SetMorph(string morph)
            {
                _pet.Morph = morph;
                return this;
            }

            public IEntityFactory.IPetBuilder SetHatchDate(DateTime date)
            {
                _pet.HatchDate = date;
                return this;
            }

            public IPet Build()
            {
                return new Pet
                {
                    Name = _pet.Name,
                    Morph = _pet.Morph,
                    Species = _pet.Species,
                    HatchDate = _pet.HatchDate
                };
            }
        }
        
        private sealed class SampleBuilder : IEntityFactory.ISampleBuilder
        {
            private readonly EnvDataSample _sample;

            public SampleBuilder()
            {
                _sample = new EnvDataSample();
            }
            
            public IEntityFactory.ISampleBuilder AddMeasurement(IEntityFactory.ISampleBuilder.MeasurementZone zone, double value)
            {
                switch (zone)
                {
                    case IEntityFactory.ISampleBuilder.MeasurementZone.HotGlass:
                        _sample.HotGlass = value;
                        break;
                    case IEntityFactory.ISampleBuilder.MeasurementZone.HotMat:
                        _sample.HotMat = value;
                        break;
                    case IEntityFactory.ISampleBuilder.MeasurementZone.MidGlass:
                        _sample.MidGlass = value;
                        break;
                    case IEntityFactory.ISampleBuilder.MeasurementZone.ColdGlass:
                        _sample.ColdGlass = value;
                        break;
                    case IEntityFactory.ISampleBuilder.MeasurementZone.ColdMat:
                        _sample.ColdMat = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(zone), zone, "Unrecognized measurement zone");
                }
                return this;
            }

            public IEntityFactory.ISampleBuilder SetInhabitant(IPet? inhabitant)
            {
                _sample.Occupant = inhabitant;
                return this;
            }

            public IEntityFactory.ISampleBuilder SetEnvironment(IEnvironment environment)
            {
                _sample.Environment = environment;
                return this;
            }

            public IEnvDataSample Build()
            {
                return new EnvDataSample
                {
                    Environment = _sample.Environment,
                    Occupant = _sample.Occupant,
                    Captured = _sample.Captured,
                    HotGlass = _sample.HotGlass,
                    HotMat = _sample.HotMat,
                    MidGlass = _sample.MidGlass,
                    ColdGlass = _sample.ColdGlass,
                    ColdMat = _sample.ColdMat
                };
            }
        }
        
        public IEntityFactory.IUserBuilder GetUserBuilder()
        {
            return new UserBuilder();
        }

        public IEntityFactory.IEnvBuilder GetEnvironmentBuilder()
        {
            return new EnvBuilder();
        }

        public IEntityFactory.IPetBuilder GetPetBuilder()
        {
            return new PetBuilder();
        }

        public IEntityFactory.ISampleBuilder GetSampleBuilder()
        {
            return new SampleBuilder();
        }
    }
}