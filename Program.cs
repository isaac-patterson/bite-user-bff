using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace user_bff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddSystemsManager("/Database"); //takes from aws pstore all parameters starting with database/
                })

            .UseStartup<Startup>()
            .UseDefaultServiceProvider(options =>
                options.ValidateScopes = false);
    }
}
