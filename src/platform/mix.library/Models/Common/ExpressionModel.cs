namespace Mix.Lib.Models.Common
{
    public class ExpressionModel
    {
        public MixLogicalOperatorKind ExpressionType { get; set; }

        public List<FunctionModel> Functions { get; set; }

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