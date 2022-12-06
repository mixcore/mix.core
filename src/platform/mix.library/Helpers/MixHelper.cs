using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mix.Lib.Helpers
{
    public sealed class MixHelper
    {
        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args) where TStartup : class
        {
            var mixContentFolder = new DirectoryInfo(MixFolders.MixContentFolder);

            // Clone Settings from shared folder
            if (!mixContentFolder.Exists)
            {
                CopyFolder(MixFolders.SharedConfigurationFolder, MixFolders.MixContentFolder);
                Console.WriteLine("Clone Settings from shared folder completed.");
            }
            return Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/azure.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/ocelot.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/storage.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/queue.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/mix_heart.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/authentication.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/google_firebase.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/smtp.json", true, true)
                       .AddJsonFile($"{MixAppConfigFilePaths.Shared}/AppConfigs/payments.json", true, true)
                       .AddEnvironmentVariables();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<TStartup>();
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
            Equals(id, default(TPrimaryKey))
            || (id is Guid && Guid.Parse(id.ToString()) == Guid.Empty) 
            || (id is int && int.Parse(id.ToString()) == 0);

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

        public static List<object> LoadEnumValues(Type enumType)
        {
            List<object> result = new();
            var values = Enum.GetValues(enumType);
            foreach (var item in values)
            {
                result.Add(Enum.ToObject(enumType, item));
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
