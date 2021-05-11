using Microsoft.Extensions.DependencyInjection;
using Viv2.API.Core.Dto.Request;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.Core.UseCases
{
    public static class ServiceExtension
    {
        public static void ApplyUseCases(this IServiceCollection services)
        {
            services.AddTransient<IAddSampleUseCase, AddSampleUseCase>();
            services.AddTransient<IAddSpeciesUseCase, AddSpeciesUseCase>();
            services.AddTransient<IAddPetUseCase, AddPetUseCase>();
            services.AddTransient<IDataProviderGrantUseCase, DataProviderGrantUseCase>();
            services.AddTransient<ILoginUseCase, LoginUseCase>();
            services.AddTransient<IRefreshTokenExchangeUseCase, RefreshTokenExchangeUseCase>();
            services.AddTransient<IAddUserUseCase, AddUserUseCase>();
            services.AddTransient<IModifyUserRolesUseCase, ModifyUserRolesUseCase>();
            services.AddTransient<IGetSamplesUseCase, GetSampleDataUseCase>();
            services.AddTransient<IGetPetDataUseCase, GetPetDataUseCase>();
            services.AddTransient<IRegisterEnvironmentUseCase, RegisterEnvironmentUseCase>();
            services.AddTransient<IGetEnvironmentsUseCase, GetEnvironmentUseCase>();
            services.AddTransient<IGetSpeciesDataUseCase, GetSpeciesDataUseCase>();
            services.AddTransient<IGetNodeControllerUseCase, GetNodeControllerUseCase>();
        }
    }
}
