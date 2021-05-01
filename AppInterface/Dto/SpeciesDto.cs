using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.AppInterface.Dto
{
    public class SpeciesDto
    {
        public static SpeciesDto From(ISpecies species)
        {
            return new()
            {
                Id = species.Id,
                Name = species.Name,
                ScientificName = species.ScientificName
            };
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
    }
}