using GraphQL;
using GraphQL.Types;
using Mix.Cms.Lib.Helpers;
using Mix.Heart.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public class TableType : ObjectGraphType<object>
    {
        public QueryArguments TableArgs
        {
            get; set;
        }
        private IDictionary<string, Type> _databaseTypeToSystemType;
        public Type _type { get; set; }
        protected IDictionary<string, Type> DatabaseTypeToSystemType
        {
            get
            {
                if (_databaseTypeToSystemType == null)
                {
                    _databaseTypeToSystemType = new Dictionary<string, Type> {
                    { "uniqueidentifier", typeof(String) },
                    { "char", typeof(String) },
                    { "nvarchar(250)", typeof(String) },
                    { "nvarchar(50)", typeof(String) },
                    { "text", typeof(String) },
                    { "int", typeof(int) },
                    { "decimal", typeof(decimal) },
                    { "bit", typeof(bool) }
                };
                }
                return _databaseTypeToSystemType;
            }
        }
        public TableType(TableMetadata tableMetadata, Type type)
        {
            _type = type;
            Name = tableMetadata.TableName;
            foreach (var tableColumn in tableMetadata.Columns)
            {
                InitGraphTableColumn(tableColumn);
            }
        }
        private void InitGraphTableColumn(ColumnMetadata columnMetadata)
        {
            var graphQLType = (ResolveColumnMetaType(columnMetadata.DataType)).GetGraphTypeFromType(true);
            var columnField = Field(
                graphQLType,
                columnMetadata.ColumnName
            ); 
            columnField.Resolver = new NameFieldResolver();
            FillArgs(columnMetadata.ColumnName);
        }
        private void FillArgs(string columnName)
        {
            var dataType = ReflectionHelper.GetPropertyType(_type, columnName);
            if (TableArgs == null)
            {
                TableArgs = new QueryArguments();
                TableArgs.Add(new QueryArgument<IntGraphType> { Name = "first" });
                TableArgs.Add(new QueryArgument<IntGraphType> { Name = "offset" });
            }
            if (dataType != null)
            {

                switch (dataType.Name)
                {
                    case "Int32":
                        TableArgs.Add(new QueryArgument<IntGraphType> { Name = columnName });
                        break;
                    case "DateTime":
                        TableArgs.Add(new QueryArgument<DateGraphType> { Name = columnName });
                        break;
                    case "Boolean":
                        TableArgs.Add(new QueryArgument<BooleanGraphType> { Name = columnName });
                        break;
                    default:
                        TableArgs.Add(new QueryArgument<StringGraphType> { Name = columnName });
                        break;
                }
            }
            //TableArgs.Add(new QueryArgument<IntGraphType> { Name = "id" });
            //TableArgs.Add(new QueryArgument<IntGraphType> { Name = "first" });
            //TableArgs.Add(new QueryArgument<IntGraphType> { Name = "offset" });
        }
        private Type ResolveColumnMetaType(string dbType)
        {
            if (DatabaseTypeToSystemType.ContainsKey(dbType))
                return DatabaseTypeToSystemType[dbType]; return typeof(String);
        }
    }
}
