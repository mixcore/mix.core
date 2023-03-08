using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Services.Graphql.Lib.Models
{
    public class TableMetadata
    {
        public string TableName { get; set; }
        public string AssemblyFullName { get; set; }
        public IEnumerable<ColumnMetadata> Columns { get; set; }
    }
}
