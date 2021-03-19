using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Models.Cms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Mix.Cms.Lib.Helpers
{
    public class EFCoreHelper
    {
        public static List<JObject> RawSqlQuery(string query, MixCmsContext context)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<JObject>();
                    var cols = new List<string>();
                    for (var i = 0; i < result.FieldCount; i++)
                        cols.Add(result.GetName(i));

                    while (result.Read())
                    {

                        entities.Add(SerializeRow(cols, result));
                    }

                    return entities;
                }
            }
        }

        private static JObject SerializeRow(IEnumerable<string> cols, DbDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return JObject.FromObject(result);
        }
    }
}
