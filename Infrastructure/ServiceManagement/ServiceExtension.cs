using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Viv2.API.Core.Adapters;
using Viv2.API.Core.ConfigModel;
using Viv2.API.Core.Constants;
using Viv2.API.Infrastructure.DataStore.EfNpgSql;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities;
using Viv2.API.Infrastructure.JwtMinting;
using Viv2.API.Infrastructure.JwtMinting.Jws;

namespace Viv2.API.Infrastructure.ServiceManagement
{
    /// <summary>
    /// Provides extension method(s) for selecting and adding particular services to a service collection.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Adds the requested minter strategy implementation to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        public static void AddTokenAuth(this IServiceCollection services, TokenMinterTypes type, IConfiguration configuration)
        {
            switch (type)
            {
                case TokenMinterTypes.JWS:
                    MinterOptions options = new MinterOptions();
                    configuration.Bind("MinterOptions", options);
                    JwsMinter minter = new JwsMinter(options);
                    services.AddSingleton<ITokenMinter>(minter);
                    _ConfigureForJwtAuth(services, configuration, minter.ValidationParameters, options);
                    break;
                case TokenMinterTypes.JWE:
                case TokenMinterTypes.PaSeTo:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Adds service implementations for the requested strategy.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="configuration">A handle on the application configuration, where information for
        /// the pertinent data backing policy can be found (such as connection strings)</param>
        public static void AddDataStore(this IServiceCollection services, BackingStoreTypes type, IConfiguration configuration)
        {
            if (type != BackingStoreTypes.EfIdent) throw new NotImplementedException();
            
            // Add the MsIDent claims composer
            services.AddSingleton<IClaimsComposer, MsIdent.ClaimsComposer>();
            
            // Add the entity factory
            services.AddTransient<IEntityFactory, EntityFactory>();
            
            // Add backing store:
            services.AddDbContext<DataContext>(options => 
                options.UseNpgsql(configuration.GetConnectionString("DataContextConnectionString")));

            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IPetStore, PetStore>();
            services.AddScoped<IEnvironmentStore, EnvironmentStore>();
            services.AddScoped<IControllerStore, ControllerStore>();

            services.AddIdentity<User, IdentityRole>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;

                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DataContext>();
        }

        private static void _ConfigureForJwtAuth(IServiceCollection services, 
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters,
            MinterOptions minterOptions)
        { 
            // Add the JWT-speicific ClaimsIdentityCompat to services:
            services.AddTransient<IClaimIdentityCompat, ClaimsIdentityCompat>();
            
            // Add authentication framework
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = minterOptions.Issuer;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = false;
            });
            
            // Add claim-based authorization policies. (Done here as we know that we'll be claim-based
            //  as we're configuring for JWTs)
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    PolicyNames.UserAccess, 
                    policy =>
                    {
                        policy.RequireClaim(ClaimNames.AccessType, AccessLevelValues.User);
                    });
                options.AddPolicy(
                    PolicyNames.DaemonAccess,
                    policy =>
                    {
                        policy.RequireClaim(ClaimNames.AccessType, AccessLevelValues.Daemon);
                    });
                options.AddPolicy(PolicyNames.AnyAuthenticated,
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });
            });
        }
    }
}
