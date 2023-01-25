using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Mix.Shared.Extensions
{
    // This class is for storing (serializable) objects in session storage and retrieving them from it.
    public static class SessonExtensions
    {
        public static void Put<T>(this ISession session, string key, T value) where T : class
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key) where T : class
        {
            string s = session.GetString(key);
            return s == null ? null : JsonSerializer.Deserialize<T>(s);
        }
    }
}
