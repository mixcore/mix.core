namespace Mix.Services.Graphql.Lib.Models
{
    public class TableMetadata
    {
        public string TableName { get; set; }
        public string AssemblyFullName { get; set; }
        public IEnumerable<ColumnMetadata> Columns { get; set; }
    }
}
