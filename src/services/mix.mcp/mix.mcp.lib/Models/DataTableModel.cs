using System.Data;

namespace Mix.MCP.Lib.Models
{
    public class DataTableModel
    {
        public string TableName { get; set; }
        public List<DataColumnModel> Columns { get; set; }
        public List<DataRowModel> Rows { get; set; }

        public static DataTableModel FromDataTable(DataTable table)
        {
            return new DataTableModel
            {
                TableName = table.TableName,
                Columns = table.Columns.Cast<DataColumn>()
                    .Select(DataColumnModel.FromDataColumn)
                    .ToList(),
                Rows = table.Rows.Cast<DataRow>()
                    .Select(DataRowModel.FromDataRow)
                    .ToList()
            };
        }
    }
}