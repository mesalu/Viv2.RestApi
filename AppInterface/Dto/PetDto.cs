using System;
using System.Diagnostics.CodeAnalysis;
using Viv2.API.Core.ProtoEntities;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;

#nullable enable

namespace Viv2.API.AppInterface.Dto
{
    /// <summary>
    /// Represents a pet in transfer.
    /// </summary>
    public class PetDto
    {
        public static PetDto From([NotNull] IPet pet)
        {
            return new PetDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Morph = pet.Morph,
                HatchDate = pet.HatchDate,
                Species = (pet.Species != null) ? SpeciesDto.From(pet.Species) : null,
                CareTakerId = pet.CareTaker.Guid,
                LatestSample = (pet.LatestSample != null) ? SampleDto.From(pet.LatestSample) : null
            };
        }
        
        public int Id { get; set; }
        public string? Name { get; init; }
        public string? Morph { get; init; }
        public DateTime? HatchDate { get; init; }
        public SpeciesDto? Species { get; init; }
        public Guid CareTakerId { get; init; }
        public SampleDto? LatestSample { get; init; }
    }
}