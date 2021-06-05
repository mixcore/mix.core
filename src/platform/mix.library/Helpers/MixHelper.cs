using System;
using System.Collections.Generic;

namespace Mix.Lib.Helpers
{
    public class MixHelper
    {
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
