using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Mix.Services.Graphql.Lib.Interfaces;
using Mix.Services.Graphql.Lib.Resolvers;
using Mix.Services.Graphql.Lib.Types;

namespace Mix.Services.Graphql.Lib
{
    public class GraphQLQuery : ObjectGraphType<object>
    {
        private IDatabaseMetadata _dbMetadata;
        private ITableNameLookup _tableNameLookup;
        private DbContext _dbContext;
        public GraphQLQuery(
         DbContext dbContext,
         IDatabaseMetadata dbMetadata,
         ITableNameLookup tableNameLookup)
        {
            _dbMetadata = dbMetadata;
            _tableNameLookup = tableNameLookup;
            _dbContext = dbContext;
            Name = "GraphQLQuery";
            var assem = _dbContext.GetType().Assembly;

            foreach (var metaTable in _dbMetadata.GetTableMetadatas())
            {
                var type = assem.GetType(metaTable.AssemblyFullName);
                var tableType = new TableType(metaTable, type);
                var friendlyTableName = metaTable.TableName;
                _tableNameLookup.GetFriendlyName(metaTable.TableName);
                AddField(new FieldType
                {
                    Name = friendlyTableName,
                    Type = tableType.GetType(),
                    ResolvedType = tableType,
                    Resolver = new MyFieldResolver(metaTable, _dbContext),
                    Arguments = new QueryArguments(tableType.TableArgs)
                });
                // lets add key to get list of current table
                var listType = new ListGraphType(tableType);
                AddField(new FieldType
                {
                    Name = $"{friendlyTableName}_list",
                    Type = listType.GetType(),
                    ResolvedType = listType,
                    Resolver = new MyFieldResolver(metaTable, _dbContext),
                    Arguments = new QueryArguments(
                        tableType.TableArgs
                    )
                });
            }
        }
    }

}
