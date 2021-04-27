using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Viv2.API.AppInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                { 
                    webBuilder.UseStartup<Startup>(); 
                    webBuilder.UseUrls("http://localhost:5000", "https://0.0.0.0:5001/");
                });
    }
}
