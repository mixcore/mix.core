using Microsoft.AspNetCore.Http;
using Mix.Heart.Extensions;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Mix.Shared.Extensions
{
    // This class is for storing (serializable) objects in session storage and retrieving them from it.
    public static class SessonExtensions
    {
        public static void Put<T>(this ISession session, string key, T value) where T : class
        {
            session.Set(key, JsonConvert.SerializeObject(value).ToByteArray());
        }

        public static T Get<T>(this ISession session, string key) where T : class
        {
            var s = session.Get(key);
            return s == null ? null : JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(s));
        }
    }
}
