using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mix.Lib.Helpers;
using Mix.Shared.Constants;
using System.IO;

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
            return MixCmsHelper.CreateHostBuilder<Startup>(args);
        }
    }
}
