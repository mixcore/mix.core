using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.Infrastructure.Models
{
    public class ExpressionModel
    {
        [JsonProperty("expressionType")]
        public string ExpressionType { get; set; }

        [JsonProperty("functions")]
        public List<FunctionModel> Functions { get; set; }

        [JsonProperty("expressions")]
        public List<ExpressionModel> Expressions { get; set; }
    }

    public class FunctionModel
    {
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        [JsonProperty("rule")]
        public string Rule { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("minValue")]
        public RangValueModel MinValue { get; set; }

        [JsonProperty("maxValue")]
        public RangValueModel MaxValue { get; set; }
    }

    public class RangValueModel
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("equal")]
        public bool Equal { get; set; }
    }
}
