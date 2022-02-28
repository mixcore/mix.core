namespace Mixcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return MixHelper.CreateHostBuilder<Startup>(args).ConfigureLogging(logging =>
            {
                logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
            });
        }
    }
}
