namespace Mixcore.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder<Startup>(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder<TStartup>(string[] args) where TStartup : class
        {
            return Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile("ocelot.json", true, true)
                       .AddEnvironmentVariables();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<TStartup>();
                });
        }
    }
}
