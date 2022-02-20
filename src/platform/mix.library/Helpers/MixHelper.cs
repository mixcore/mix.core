using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mix.Lib.Helpers
{
    public class MixHelper
    {

        public static IHostBuilder CreateHostBuilder<Startup>(string[] args)
            where Startup : class
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

        public static string SerializeObject(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }
        public static FileModel GetFileModel(IFormFile file, string folder)
        {
            var result = new FileModel()
            {
                Filename = file.FileName[..file.FileName.LastIndexOf('.')],
                Extension = file.FileName[file.FileName.LastIndexOf('.')..],
                FileFolder = folder,
            };
            return result;
        }
        public static List<object> ParseEnumToObject(Type enumType)
        {
            List<object> result = new();
            var values = Enum.GetValues(enumType);
            foreach (var item in values)
            {
                result.Add(new { name = Enum.GetName(enumType, item), value = Enum.ToObject(enumType, item) });
            }
            return result;
        }

        public static void HandleException<TResult>(TResult result, Exception ex)
        {
            LogException(ex);
        }

        private static void LogException(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
