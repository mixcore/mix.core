using Npgsql;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Exceptions;
using RepoDb.Extensions;
using RepoDb.Interfaces;
using RepoDb.Resolvers;
using RepoDb.StatementBuilders;
using System.Data;
using System.Text.RegularExpressions;

namespace Mix.RepoDb.Repositories
{

    /// <summary>
    /// A class used to build a SQL Statement for SQL Server. This is the default statement builder used by the library.
    /// </summary>
    public sealed class OptimizedPostgresSqlStatementBuilder : BaseStatementBuilder
    {
        Regex regex = new("(\"\\w+\")\\s(LIKE)");
        Regex regexOrderByAsc = new("((ASC)|(asc))\\s");
        Regex regexOrderByDesc = new("((DESC)|(desc))\\s");
        bool _isUnAccend;
        /// <summary>
        /// Creates a new instance of <see cref="OptimizedPostgresSqlStatementBuilder"/> object.
        /// </summary>
        public OptimizedPostgresSqlStatementBuilder(bool isAccend)
        : base(DbSettingMapper.Get<NpgsqlConnection>(),
            new PostgreSqlConvertFieldResolver(),
            new ClientTypeToAverageableClientTypeResolver())
        {
            _isUnAccend = isAccend;
        }
        //
        // Summary:
        //     Creates a new instance of RepoDb.StatementBuilders.PostgreSqlStatementBuilder
        //     class.
        //
        // Parameters:
        //   dbSetting:
        //     The database settings object currently in used.
        //
        //   convertFieldResolver:
        //     The resolver used when converting a field in the database layer.
        //
        //   averageableClientTypeResolver:
        //     The resolver used to identity the type for average.
        public OptimizedPostgresSqlStatementBuilder(IDbSetting dbSetting, IResolver<Field, IDbSetting, string> convertFieldResolver = null, IResolver<Type, Type> averageableClientTypeResolver = null, bool isUnAccend = false)
            : base(dbSetting, convertFieldResolver, averageableClientTypeResolver)
        {
            _isUnAccend = isUnAccend;
        }


        #region Create Count

        public override string CreateCount(string tableName, QueryGroup where = null, string hints = null)
        {
            string query = base.CreateCount(tableName, where, hints);
            return ReplaceFilter(query, where);
        }
        #endregion

        #region Create Batch Query
        //
        // Summary:
        //     Creates a SQL Statement for batch query operation.
        //
        // Parameters:
        //   tableName:
        //     The name of the target table.
        //
        //   fields:
        //     The list of fields to be queried.
        //
        //   page:
        //     The page of the batch.
        //
        //   rowsPerBatch:
        //     The number of rows per batch.
        //
        //   orderBy:
        //     The list of fields for ordering.
        //
        //   where:
        //     The query expression.
        //
        //   hints:
        //     The table hints to be used.
        //
        // Returns:
        //     A sql statement for batch query operation.

        public override string CreateBatchQuery(string tableName, IEnumerable<Field> fields, int page, int rowsPerBatch, IEnumerable<OrderField> orderBy = null, QueryGroup where = null, string hints = null)
        {
            GuardTableName(tableName);
            GuardHints(hints);
            if (fields == null || !fields.Any())
            {
                throw new EmptyException("The list of queryable fields must not be null for '" + tableName + "'.");
            }

            if (orderBy == null || !orderBy.Any())
            {
                throw new EmptyException("The argument 'orderBy' is required.");
            }

            if (page < 0)
            {
                throw new ArgumentOutOfRangeException("page", "The page must be equals or greater than 0.");
            }

            if (rowsPerBatch < 1)
            {
                throw new ArgumentOutOfRangeException("rowsPerBatch", "The rows per batch must be equals or greater than 1.");
            }

            int value = page * rowsPerBatch;
            QueryBuilder queryBuilder = new QueryBuilder();
            queryBuilder.Clear().Select().FieldsFrom(fields, base.DbSetting)
                .From()
                .TableNameFrom(tableName, base.DbSetting)
                .WhereFrom(where, base.DbSetting)
                .OrderByFrom(orderBy, base.DbSetting)
                .LimitOffset(rowsPerBatch, value)
                .End();

            // Return the query
            return ReplaceFilter(queryBuilder.GetString(), where);
        }

        private string ReplaceFilter(string query, QueryGroup where = null)
        {
             if (where != null && _isUnAccend)
            {
                query = regex.Replace(query, "unaccent($1) ILIKE");
                foreach (var item in where.QueryFields)
                {
                    if (item.Operation == Operation.Like)
                    {
                        query = query.Replace($"@{item.GetName()}", $"unaccent('{item.GetValue()}')");
                    }
                }
            }
            return query;
        }


        #endregion

        #region Create Merge

        //
        // Summary:
        //     Creates a SQL Statement for merge operation.
        //
        // Parameters:
        //   tableName:
        //     The name of the target table.
        //
        //   fields:
        //     The list of fields to be merged.
        //
        //   qualifiers:
        //     The list of the qualifier RepoDb.Field objects.
        //
        //   primaryField:
        //     The primary field from the database.
        //
        //   identityField:
        //     The identity field from the database.
        //
        //   hints:
        //     The table hints to be used.
        //
        // Returns:
        //     A sql statement for merge operation.
        public override string CreateMerge(string tableName, IEnumerable<Field> fields, IEnumerable<Field> qualifiers = null, DbField primaryField = null, DbField identityField = null, string hints = null)
        {
            GuardTableName(tableName);
            GuardHints(hints);
            GuardPrimary(primaryField);
            GuardIdentity(identityField);
            if (fields == null || !fields.Any())
            {
                throw new EmptyException("The list of fields cannot be null or empty.");
            }

            IEnumerable<Field> enumerable = qualifiers;
            if ((enumerable == null || !enumerable.Any()) && primaryField != null)
            {
                qualifiers = primaryField.AsField().AsEnumerable();
            }

            IEnumerable<Field> enumerable2 = qualifiers;
            if (enumerable2 == null || !enumerable2.Any())
            {
                if (primaryField == null)
                {
                    throw new PrimaryFieldNotFoundException("The is no primary field from the table '" + tableName + "' that can be used as qualifier.");
                }

                throw new InvalidQualifiersException("There are no defined qualifier fields.");
            }

            QueryBuilder queryBuilder = new QueryBuilder();
            List<Field> fields2 = fields.Where(delegate (Field f)
            {
                IEnumerable<Field> enumerable3 = qualifiers;
                return enumerable3 == null || !enumerable3.Any((Field qf) => string.Equals(qf.Name, f.Name, StringComparison.OrdinalIgnoreCase));
            }).AsList();
            queryBuilder.Clear().Insert().Into()
                .TableNameFrom(tableName, base.DbSetting)
                .OpenParen()
                .FieldsFrom(fields, base.DbSetting)
                .CloseParen();
            if (identityField != null)
            {
                queryBuilder.WriteText("OVERRIDING SYSTEM VALUE");
            }

            queryBuilder.Values().OpenParen().ParametersFrom(fields, 0, base.DbSetting)
                .CloseParen()
                .OnConflict(qualifiers, base.DbSetting)
                .DoUpdate()
                .Set()
                .FieldsAndParametersFrom(fields2, 0, base.DbSetting);
            DbField returnKeyColumnAsDbField = GetReturnKeyColumnAsDbField(primaryField, identityField);
            string text = "NULL";
            if (returnKeyColumnAsDbField != null)
            {
                string databaseType = GetDatabaseType(returnKeyColumnAsDbField);
                text = (string.IsNullOrWhiteSpace(databaseType) ? returnKeyColumnAsDbField.Name.AsQuoted(base.DbSetting) : $"CAST({returnKeyColumnAsDbField.Name.AsQuoted(base.DbSetting)} AS {databaseType})");
            }

            string text2 = "RETURNING " + text + " AS " + "Result".AsQuoted(base.DbSetting);
            queryBuilder.WriteText(text2);
            queryBuilder.End();
            return queryBuilder.GetString();
        }

        #endregion

        #region Create Merge all

        //
        // Summary:
        //     Creates a SQL Statement for merge-all operation.
        //
        // Parameters:
        //   tableName:
        //     The name of the target table.
        //
        //   fields:
        //     The list of fields to be merged.
        //
        //   qualifiers:
        //     The list of the qualifier RepoDb.Field objects.
        //
        //   batchSize:
        //     The batch size of the operation.
        //
        //   primaryField:
        //     The primary field from the database.
        //
        //   identityField:
        //     The identity field from the database.
        //
        //   hints:
        //     The table hints to be used.
        //
        // Returns:
        //     A sql statement for merge operation.
        public override string CreateMergeAll(string tableName, IEnumerable<Field> fields, IEnumerable<Field> qualifiers, int batchSize = 10, DbField primaryField = null, DbField identityField = null, string hints = null)
        {
            GuardTableName(tableName);
            GuardHints(hints);
            GuardPrimary(primaryField);
            GuardIdentity(identityField);
            if (fields == null || !fields.Any())
            {
                throw new EmptyException("The list of fields cannot be null or empty.");
            }

            IEnumerable<Field> enumerable = qualifiers;
            if ((enumerable == null || !enumerable.Any()) && primaryField != null)
            {
                qualifiers = primaryField.AsField().AsEnumerable();
            }

            IEnumerable<Field> enumerable2 = qualifiers;
            if (enumerable2 == null || !enumerable2.Any())
            {
                if (primaryField == null)
                {
                    throw new PrimaryFieldNotFoundException("The is no primary field from the table '" + tableName + "' that can be used as qualifier.");
                }

                throw new InvalidQualifiersException("There are no defined qualifier fields.");
            }

            QueryBuilder queryBuilder = new QueryBuilder();
            List<Field> fields2 = fields.Where(delegate (Field f)
            {
                IEnumerable<Field> enumerable3 = qualifiers;
                return enumerable3 == null || !enumerable3.Any((Field qf) => string.Equals(qf.Name, f.Name, StringComparison.OrdinalIgnoreCase));
            }).AsList();
            DbField returnKeyColumnAsDbField = GetReturnKeyColumnAsDbField(primaryField, identityField);
            string text = "NULL";
            if (returnKeyColumnAsDbField != null)
            {
                string databaseType = GetDatabaseType(returnKeyColumnAsDbField);
                text = (string.IsNullOrWhiteSpace(databaseType) ? returnKeyColumnAsDbField.Name.AsQuoted(base.DbSetting) : $"CAST({returnKeyColumnAsDbField.Name.AsQuoted(base.DbSetting)} AS {databaseType})");
            }

            queryBuilder.Clear();
            for (int i = 0; i < batchSize; i++)
            {
                queryBuilder.Insert().Into().TableNameFrom(tableName, base.DbSetting)
                    .OpenParen()
                    .FieldsFrom(fields, base.DbSetting)
                    .CloseParen();
                if (identityField != null)
                {
                    queryBuilder.WriteText("OVERRIDING SYSTEM VALUE");
                }

                queryBuilder.Values().OpenParen().ParametersFrom(fields, i, base.DbSetting)
                    .CloseParen()
                    .OnConflict(qualifiers, base.DbSetting)
                    .DoUpdate()
                    .Set()
                    .FieldsAndParametersFrom(fields2, i, base.DbSetting);
                string text2 = "RETURNING " + text + " AS " + "Result".AsQuoted(base.DbSetting) + ", " + $"{base.DbSetting.ParameterPrefix}__RepoDb_OrderColumn_{i}" + " AS " + "OrderColumn".AsQuoted(base.DbSetting);
                queryBuilder.WriteText(text2);
                queryBuilder.End();
            }

            return queryBuilder.GetString();
        }

        #endregion

        #region Create Skip Query

        //
        // Summary:
        //     Creates a SQL Statement for 'BatchQuery' operation.
        //
        // Parameters:
        //   tableName:
        //     The name of the target table.
        //
        //   fields:
        //     The mapping list of RepoDb.Field objects to be used.
        //
        //   skip:
        //     The number of rows to skip.
        //
        //   take:
        //     The number of rows per batch.
        //
        //   orderBy:
        //     The list of fields for ordering.
        //
        //   where:
        //     The query expression.
        //
        //   hints:
        //     The table hints to be used.
        //
        // Returns:
        //     A sql statement for batch query operation.
        public override string CreateSkipQuery(string tableName, IEnumerable<Field> fields, int skip, int take, IEnumerable<OrderField> orderBy = null, QueryGroup where = null, string hints = null)
        {
            GuardTableName(tableName);
            GuardHints(hints);
            if (fields == null || !fields.Any())
            {
                throw new EmptyException("The list of queryable fields must not be null for '" + tableName + "'.");
            }

            if (orderBy == null || !orderBy.Any())
            {
                throw new EmptyException("The argument 'orderBy' is required.");
            }

            if (skip < 0)
            {
                throw new ArgumentOutOfRangeException("skip", "The rows skipped must be equals or greater than 0.");
            }

            if (take < 1)
            {
                throw new ArgumentOutOfRangeException("take", "The rows per batch must be equals or greater than 1.");
            }

            QueryBuilder queryBuilder = new QueryBuilder();
            queryBuilder.Clear().Select().FieldsFrom(fields, base.DbSetting)
                .From()
                .TableNameFrom(tableName, base.DbSetting)
                .WhereFrom(where, base.DbSetting)
                .OrderByFrom(orderBy, base.DbSetting)
                .LimitOffset(take, skip)
                .End();
            return queryBuilder.GetString();
        }

        #endregion

        #region Private

        private string GetDatabaseType(DbField dbField)
        {
            DbType? dbType = new ClientTypeToDbTypeResolver().Resolve(dbField.Type);
            if (!dbType.HasValue)
            {
                return null;
            }

            return new DbTypeToPostgreSqlStringNameResolver().Resolve(dbType.Value);
        }
        #endregion


    }
}