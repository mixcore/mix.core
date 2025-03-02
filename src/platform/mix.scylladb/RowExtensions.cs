using Cassandra;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mix.Scylladb
{
    public static class RowExtensions
    {
        public static JObject ParseJObject(this Row row, CqlColumn[] columns)
        {
            JObject result = new JObject();
            foreach (var col in columns)
            {
                result.Add(new JProperty(col.Name, row[col.Name]));
            }
            return result;
        }
        
        public static List<JObject>? ParseListJObject(this RowSet? rows)
        {
            if (rows == null) return default;

            var result = new List<JObject>();
            foreach (var row in rows)
            {
                result.Add(ParseJObject(row, rows.Columns));
            }
            return result;
        }
    }
}
