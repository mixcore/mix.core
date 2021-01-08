using Newtonsoft.Json;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models
{
    public class FunctionModel
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        [JsonProperty("rule")]
        public string Rule { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("minValue")]
        public string MinValue { get; set; }

        [JsonProperty("maxValue")]
        public string MaxValue { get; set; }

        public static FunctionModel Create(string rule, string fieldName, string value)
        {
            return new FunctionModel()
            {
                Rule = rule,
                FieldName = fieldName,
                Value = value,
            };

        }

        public static FunctionModel Create(string rule, string fieldName,
           string minValue = "", string maxValue = "")
        {
            return new FunctionModel()
            {
                Rule = rule,
                FieldName = fieldName,
                MinValue = minValue,
                MaxValue = maxValue,
            };
        }
    }
}
