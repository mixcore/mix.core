using Mix.Lib.Helpers;

namespace SampleAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            MixCmsHelper.CreateHostBuilder<Startup>(args);
    }
}
