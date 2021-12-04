using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Mix.Lib.Helpers
{
    public class MixCmsHelper
    {
        public static IHostBuilder CreateHostBuilder<Startup>(string[] args)
            where Startup: class
        {
            if (!Directory.Exists(MixFolders.ConfiguratoinFolder))
            {
                CopyFolder(MixFolders.SharedConfigurationFolder, MixFolders.ConfiguratoinFolder);
            }
            return Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/database.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/ocelot.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/queue.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/mix_heart.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/authentication.json", true, true)
                       .AddEnvironmentVariables();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                });
        }

        public static bool CopyFolder(string srcPath, string desPath)
        {
            if (srcPath.ToLower() != desPath.ToLower() && Directory.Exists(srcPath))
            {
                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(srcPath, desPath));
                }

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(srcPath, desPath), true);
                }

                return true;
            }
            return true;
        }

        public static bool IsDefaultId<TPrimaryKey>(TPrimaryKey id) => 
            id == null 
            || (id.GetType() == typeof(Guid) && Guid.Parse(id.ToString()) == Guid.Empty)
            || (id.GetType() == typeof(int) && int.Parse(id.ToString()) == 0);
    }
}
