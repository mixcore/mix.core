using CommandLine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mix.Lib.Helpers
{
    public class MixHelper
    {
        public class Options
        {
            [Option('c', "clean", Required = false, HelpText = "Clean installed Mixcore CMS version.")]
            public bool Clean { get; set; }
        }

        public static IHostBuilder CreateHostBuilder<Startup>(string[] args)
            where Startup : class
        {

            var mixContentFolder = new DirectoryInfo(MixFolders.MixContentFolder);

            // Mixcore Cli
            MixCli(args);

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
                       .AddJsonFile("MixContent/AppConfigs/azure.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/ocelot.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/queue.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/mix_heart.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/authentication.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/google_firebase.json", true, true)
                       .AddJsonFile("MixContent/AppConfigs/smtp.json", true, true)
                       .AddEnvironmentVariables();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                });
        }

        public static void MixCli(string[] args)
        {
            var mixContentFolder = new DirectoryInfo(MixFolders.MixContentFolder);

            // Parse Mixcore cli
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       // Check if clean before run is required
                       if (o.Clean)
                       {
                           Console.WriteLine($"Current Arguments: -v {o.Clean}");

                           if (mixContentFolder.Exists)
                           {
                               // Delete existing MixContent folder
                               Console.WriteLine("Do you want to clean all installed previous Mixcore CMS settings! (y/n):");
                               string isClean = Console.ReadLine().ToLower();
                               if (isClean.Equals("y"))
                               {
                                   try
                                   {
                                       mixContentFolder.Delete(true);
                                       Console.WriteLine("Clean completed! Continue to web interface.");
                                   }
                                   catch (IOException ex)
                                   {
                                       Console.WriteLine(ex.Message);
                                   }
                               }
                           }
                       }
                       else
                       {
                           //Console.WriteLine($"Current Arguments: -v {o.Clean}");
                           Console.WriteLine("There is no cli arg! Continue to web interface.");
                       }
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
object.Equals(id, default(TPrimaryKey))
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
