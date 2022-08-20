namespace Mix.Lib.Models.Common
{
    public class FunctionModel
    {
        public string FieldName { get; set; }

        public MixCompareOperator Rule { get; set; }

        public string Value { get; set; }

        public string MinValue { get; set; }

        public string MaxValue { get; set; }

        public static FunctionModel Create(MixCompareOperator rule, string fieldName, string value)
        {
            return new FunctionModel()
            {
                Rule = rule,
                FieldName = fieldName,
                Value = value,
            };
        }

        public static FunctionModel Create(MixCompareOperator operatorKind, string fieldName,
           string minValue = "", string maxValue = "")
        {
            return new FunctionModel()
            {
                Rule = operatorKind,
                FieldName = fieldName,
                MinValue = minValue,
                MaxValue = maxValue,
            };
        }
    }
}