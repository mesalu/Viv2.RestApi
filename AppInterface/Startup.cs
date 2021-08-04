using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Viv2.API.Core.UseCases;
using Viv2.API.Infrastructure.ServiceManagement;

namespace Viv2.API.AppInterface
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Apply Core's default use cases.
            services.ApplyUseCases();
            
            // Add supporting services.
            /* TODO: These functions un-"onion" the project (imposes a dependency through core)
             *       so ideally we want to find a better way to do this.
             */
            services.AddRelationalDataStore(BackingStoreTypes.EfIdent, Configuration);
            services.AddTokenAuth(TokenMinterTypes.JWS, Configuration);
            services.AddBlobDataStore(Configuration);
            
            // Configure CORS policies (depending on mode)
#if DEBUG
            services.AddCors(options =>
            {
                options.AddPolicy("LocalHost3000",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
#else
            services.AddCors(options =>
            {
                options.AddPolicy("BlobStorageStaticSite",
                    builder =>
                    {
                        builder.WithOrigins("TBD")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });
#endif
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            
            app.UseRequestLogging();
#if DEBUG
            app.UseCors("LocalHost3000");
#else
            app.UseCors("BlobStorageStaticSite");
#endif  
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
