using Microsoft.AspNetCore.Http;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mix.Lib.Helpers
{
    public class MixHelper
    {
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
