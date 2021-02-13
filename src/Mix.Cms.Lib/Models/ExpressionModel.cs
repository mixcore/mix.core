using Mix.Cms.Lib.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models
{
    public class ExpressionModel
    {
        [JsonProperty("expressionType")]
        public MixLogicalOperatorKind ExpressionType { get; set; }

        [JsonProperty("functions")]
        public List<FunctionModel> Functions { get; set; }

        [JsonProperty("expressions")]
        public List<ExpressionModel> Expressions { get; set; }

        public static ExpressionModel Create(MixLogicalOperatorKind expressType)
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
}