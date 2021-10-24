using Microsoft.AspNetCore.Http;
using Mix.Heart.Models;
using System;
using System.Collections.Generic;

namespace Mix.Lib.Helpers
{
    public class MixHelper
    {
        public static FileModel GetFileModel(IFormFile file, string folder)
        {
            return new FileModel()
            {
                Filename = file.FileName.Substring(0, file.FileName.LastIndexOf('.')),
                Extension = file.FileName.Substring(file.FileName.LastIndexOf('.')),
                FileFolder = folder
            };
        }
        public static List<object> ParseEnumToObject(Type enumType)
        {
            List<object> result = new List<object>();
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
