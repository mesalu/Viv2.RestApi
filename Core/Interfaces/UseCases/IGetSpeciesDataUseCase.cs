using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;

using TContent = Viv2.API.Core.ProtoEntities.ISpecies;

namespace Viv2.API.Core.Interfaces.UseCases
{
    public interface IGetSpeciesDataUseCase : IUseCaseRequestHandler<DataAccessRequest<TContent>, GenericDataResponse<TContent>>
    {
        
    }
}