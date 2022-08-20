using Mix.Constant.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Constant.Dtos
{
    public class SearchQueryField
    {
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
    }
}
