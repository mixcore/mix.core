using System.Data;

namespace Mix.MCP.Lib.Models
{
    public class DataColumnModel
    {
        public string ColumnName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
        public string DefaultValue { get; set; } = string.Empty;

        public static DataColumnModel FromDataColumn(DataColumn column)
        {
            return new DataColumnModel
            {
                ColumnName = column.ColumnName,
                DataType = column.DataType.Name,
                IsNullable = column.AllowDBNull,
                DefaultValue = column.DefaultValue?.ToString() ?? string.Empty
            };
        }
    }
}