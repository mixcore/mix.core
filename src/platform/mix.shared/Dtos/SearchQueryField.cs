using System.ComponentModel.DataAnnotations;

namespace Mix.Shared.Dtos
{
    public class SearchQueryField
    {
        public SearchQueryField()
        {

        }
        public SearchQueryField(string fieldName, string value, MixCompareOperator compareOperation = MixCompareOperator.Equal)
        {
            FieldName = fieldName;
            Value = value.ToString();
            CompareOperator = compareOperation;
        }
        [Required]
        public string FieldName { get; set; }
        [Required]
        public string Value { get; set; }
        public MixCompareOperator CompareOperator { get; set; }
        public bool IsRequired { get; set; }
    }
}
