using Mix.Lib.Helpers;
using Mix.Services.Databases;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = MixCmsHelper.CreateHostBuilder<Startup>(args);
        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("mixcontent/shared/appconfigs/kiotviet.json", true, true);
        });
        return builder;
    }
}