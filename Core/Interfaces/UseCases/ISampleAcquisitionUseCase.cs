using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Dto.Response;
using Viv2.API.Core.ProtoEntities;

namespace Viv2.API.Core.Interfaces.UseCases
{
    /// <summary>
    /// Acquiring samples from storage is one of the most expensive things this api does, and it will be doing a lot of
    /// it. As such the (current) default data access approach is a bit slow & relies too heavily on local processing
    /// power (as selection predicates don't get mapped through LINQ, all the filtering happens locally).
    /// This UseCase is purpose built by implementations to be as lean on local processing as possible. 
    /// </summary>
    public interface ISampleAcquisitionUseCase : IUseCaseRequestHandler<SampleAccessRequest, GenericDataResponse<IEnvDataSample>>
    {
        
    }
}