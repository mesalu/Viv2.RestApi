using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;

// Avoid excessive re-use and chained generic types.
using TRequestInfo = System.Collections.Generic.IList<Viv2.API.Core.ProtoEntities.IEnvironment>;

namespace Viv2.API.Core.Interfaces.UseCases
{
    /// <summary>
    /// Used for accessing 
    /// </summary>
    public interface IGetEnvironmentsUseCase : IUseCaseRequestHandler<DataAccessRequest<TRequestInfo>, GenericDataResponse<TRequestInfo>>
    {
    }
}