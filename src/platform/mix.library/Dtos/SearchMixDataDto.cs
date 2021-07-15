using Mix.Heart.Enums;
using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System;

namespace Mix.Lib.Dtos
{
    public class SearchMixDataDto : SearchRequestDto
    {
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixCompareOperatorKind CompareKind { get; set; }
        public bool IsGroup { get; set; }
        public JObject Fields { get; set; }
    }
}
