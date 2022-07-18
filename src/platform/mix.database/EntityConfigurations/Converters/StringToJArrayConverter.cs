using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace Mix.Database.EntityConfigurations.Converters
{
    public class StringToJArrayConverter : ValueConverter<JArray, string>
    {
        public StringToJArrayConverter() 
            : base(v => v.ToString(),
            v => JArray.Parse(v ?? "[]"))
        {

        }
    }
}
