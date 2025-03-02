using System.ComponentModel.DataAnnotations;

namespace Mix.Shared.Dtos
{
    public class SearchQueryField
    {
        public SearchQueryField()
        {

        }
        public SearchQueryField(string fieldName, object value, MixCompareOperator compareOperation = MixCompareOperator.Equal)
        {
            FieldName = fieldName;
            Value = value;
            CompareOperator = compareOperation;
        }

        [Required]
        public string FieldName { get; set; }

        public object Value { get; set; }

        public MixCompareOperator CompareOperator { get; set; }

        public bool IsRequired { get; set; }
    }
}
