using Mix.Lib.Helpers;

namespace Mix.Workflow.Api
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
