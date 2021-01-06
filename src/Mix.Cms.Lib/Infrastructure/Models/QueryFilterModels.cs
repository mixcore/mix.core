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


        public static ExpressionModel Create(string expressType)
        {
            return new ExpressionModel() { ExpressionType = expressType };
        }

        public ExpressionModel AddFunction(FunctionModel function)
        {
            (this.Functions ??= new List<FunctionModel>()).Add(function);
            return this;
        }

        public ExpressionModel AddExpression(ExpressionModel expression)
        {
            (this.Expressions ??= new List<ExpressionModel>()).Add(expression);
            return this;
        }
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
