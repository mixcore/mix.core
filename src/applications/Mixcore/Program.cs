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
            var builder =  MixCmsHelper.CreateHostBuilder<Startup>(args);
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("MixContent/Shared/AppConfigs/kiotviet.json", true, true);
            });
            return builder;
        }
    }
}
