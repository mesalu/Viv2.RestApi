using System;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.Interfaces;

namespace Viv2.API.Core.Dto.Request
{
    public class NewPetRequest : IUseCaseRequest<NewEntityResponse<string>>
    {
        public string Name { get; set; }
        public string Morph { get; set; }
        
        /// <summary>
        /// The Guid value of a species, as selected from the last call to get all species data. 
        /// </summary>
        public int SpeciesType { get; set; }
        
        /// <summary>
        /// Guid indicating the user creating the pet.
        /// </summary>
        public Guid User { get; set; }
    }
}