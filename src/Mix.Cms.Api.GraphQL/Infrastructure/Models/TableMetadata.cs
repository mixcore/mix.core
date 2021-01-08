using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public class TableMetadata
    {
        public string TableName { get; set; }
        public string AssemblyFullName { get; set; }
        public IEnumerable<ColumnMetadata> Columns { get; set; }
    }
}
