using System.Data;

namespace Mix.MCP.Lib.Models
{
    public class DataRowModel
    {
        public Dictionary<string, object> Values { get; set; } = new();

        public static DataRowModel FromDataRow(DataRow row)
        {
            var values = new Dictionary<string, object>();
            foreach (DataColumn column in row.Table.Columns)
            {
                values[column.ColumnName] = row[column] is DBNull ? string.Empty : row[column];
            }
            return new DataRowModel { Values = values };
        }
    }
}