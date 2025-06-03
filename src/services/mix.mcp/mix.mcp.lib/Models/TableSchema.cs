using System.Collections.Generic;

namespace Mix.MCP.Lib.Models
{
    public class TableSchema
    {
        public string TableName { get; set; } = string.Empty;
        public List<ColumnSchema> Columns { get; set; } = new();
    }

    public class ColumnSchema
    {
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public string? DefaultValue { get; set; }
    }
}