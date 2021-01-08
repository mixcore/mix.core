using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public class ColumnMetadata
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }
}
