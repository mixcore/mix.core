using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Api.GraphQL.Infrastructure.Interfaces;
using System;
using System.Reflection;

namespace Mix.Cms.Api.GraphQL.Infrastructure
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
            Name = "Query"; 
            foreach (var metaTable in _dbMetadata.GetTableMetadatas())
            {
                var tableType = new TableType(metaTable);
                var friendlyTableName = metaTable.TableName;                
                // _tableNameLookup.GetFriendlyName(metaTable.TableName); 
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
                var type = listType.GetType();
                AddField(new FieldType
                {
                    Name = $"{friendlyTableName}_list",
                    Type = type,
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
