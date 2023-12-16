using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities.Federation;
using Mix.Heart.Helpers;
using Mix.Services.Graphql.Lib.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Graphql.Lib.Types
{
    public class TableType : ObjectGraphType<object>
    {
        public QueryArguments TableArgs
        {
            get; set;
        }
        public Type _type { get; set; }
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
            var dataType = ReflectionHelper.GetPropertyType(_type, columnMetadata.ColumnName);
            var graphQLType = ResolveColumnMetaType(dataType).GetGraphTypeFromType(true);
            Field(columnMetadata.ColumnName, graphQLType);
            FillArgs(dataType, columnMetadata.ColumnName);
        }
        private void FillArgs(Type dataType, string columnName)
        {
            if (TableArgs == null)
            {
                TableArgs = new QueryArguments
                {
                    new QueryArgument<IntGraphType> { Name = "first" },
                    new QueryArgument<IntGraphType> { Name = "offset" }
                };
            }
            if (dataType != null)
            {

                switch (dataType)
                {
                    case var n when n == typeof(Enum):
                        TableArgs.Add(new QueryArgument<EnumerationGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(JArray):
                        TableArgs.Add(new QueryArgument<AnyScalarGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(JObject):
                        TableArgs.Add(new QueryArgument<ObjectGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(Guid) || n == typeof(Guid?):
                        TableArgs.Add(new QueryArgument<GuidGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(long) || n == typeof(long?):
                        TableArgs.Add(new QueryArgument<LongGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(int) || n == typeof(int?):
                        TableArgs.Add(new QueryArgument<IntGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(DateTime) || n == typeof(DateTime?):
                        TableArgs.Add(new QueryArgument<DateTimeGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(bool) || n == typeof(bool?):
                        TableArgs.Add(new QueryArgument<BooleanGraphType> { Name = columnName });
                        break;
                    case var n when n == typeof(double) || n == typeof(double?) || n == typeof(decimal) || n == typeof(decimal?):
                        TableArgs.Add(new QueryArgument<DecimalGraphType> { Name = columnName });
                        break;
                    default:
                        TableArgs.Add(new QueryArgument<StringGraphType> { Name = columnName });
                        break;
                }
            }
        }
        private Type ResolveColumnMetaType(Type dataType)
        {
            if (dataType == typeof(JArray) || dataType == typeof(JObject))
            {
                return typeof(string);
            }
            return dataType;
        }
    }
}
