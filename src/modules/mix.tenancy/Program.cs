using Mix.Lib.Helpers;

namespace Mix.Tenancy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            MixHelper.CreateHostBuilder<Startup>(args);
    }
}
