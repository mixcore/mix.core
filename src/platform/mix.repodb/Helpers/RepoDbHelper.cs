using Mix.Constant.Enums;
using Newtonsoft.Json.Linq;
using RepoDb.Enumerations;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.RepoDb.Helpers
{
    public class RepoDbHelper
    {
        public static JObject? ParseJObject(dynamic obj)
        {
            if (obj == null)
                return null;

            var tmp = obj as IDictionary<string, object>;
            if (tmp == null)
                return null;

            var result = new JObject();
            foreach (var (key, value) in tmp)
            {
                result.Add(new JProperty(key, value));
            }
            return result;
        }

        public static List<JObject>? ParseListJObject(IEnumerable<dynamic>? data)
        {
            if (data == null) return default;

            var result = new List<JObject>();
            foreach (var row in data)
            {
                result.Add(ParseJObject(row));
            }
            return result;
        }


        public static Operation ParseOperator(MixCompareOperator compareOperator)
        {
            switch (compareOperator)
            {
                case MixCompareOperator.Equal:
                    return Operation.Equal;
                case MixCompareOperator.Like:
                case MixCompareOperator.ILike:
                    return Operation.Like;
                case MixCompareOperator.NotEqual:
                    return Operation.NotEqual;
                case MixCompareOperator.Contain:
                    return Operation.In;
                case MixCompareOperator.NotContain:
                    return Operation.NotIn;
                case MixCompareOperator.InRange:
                    return Operation.In;
                case MixCompareOperator.NotInRange:
                    return Operation.NotIn;
                case MixCompareOperator.GreaterThanOrEqual:
                    return Operation.GreaterThanOrEqual;
                case MixCompareOperator.GreaterThan:
                    return Operation.GreaterThan;
                case MixCompareOperator.LessThanOrEqual:
                    return Operation.LessThanOrEqual;
                case MixCompareOperator.LessThan:
                    return Operation.LessThan;
                default:
                    return Operation.Equal;
            }
        }

    }
}
