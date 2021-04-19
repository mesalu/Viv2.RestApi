using Microsoft.Extensions.DependencyInjection;
using Viv2.API.Core.Interfaces.UseCases;

namespace Viv2.API.Core.UseCases
{
    public static class ServiceExtension
    {
        public static void ApplyUseCases(this IServiceCollection services)
        {
            services.AddTransient<IDataProviderGrantUseCase, DataProviderGrantUseCase>();
            services.AddTransient<ILoginUseCase, LoginUseCase>();
            services.AddTransient<IRefreshTokenExchangeUseCase, RefreshTokenExchangeUseCase>();
            services.AddTransient<IAddUserUseCase, AddUserUseCase>();
            services.AddTransient<IModifyUserRolesUseCase, ModifyUserRolesUseCase>();
            services.AddTransient<IGetSamplesUseCase, GetSampleDataUseCase>();
        }
    }
}
