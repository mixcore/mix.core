using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Extensions
{
    public static class MigrationBuilderExtensions
    {
        public static CreateTableBuilder<T> TryCreateTable<T>(this MigrationBuilder builder,
            string name,
            Func<ColumnsBuilder, T> columns,
            string? schema = null,
            Action<CreateTableBuilder<T>>? constraints = null,
            string? comment = null)
        {
            try
            {
                return builder.CreateTable(name, columns, schema, constraints, comment);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot create table {name}: {ex.Message}");
            }
            return default;
        }
    }
}
