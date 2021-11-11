using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Mix.Lib.Helpers;

namespace Mix.Portal
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
