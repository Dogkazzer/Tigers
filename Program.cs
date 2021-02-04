using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;



namespace Tigers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            //var host = CreateWebHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();


            RunSeeding(host);


            host.Run();
        }

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
            var seeder = scope.ServiceProvider.GetService<TigerSeeder>();
                seeder.SeedAsync().Wait();  //do the seeding, and wait til done, this acts as a blocker, instead of seeding whilst trying to bring up the web server
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
            webBuilder.UseStartup<Startup>();
            });
    }
}
