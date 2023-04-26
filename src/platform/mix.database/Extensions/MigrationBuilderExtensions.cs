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
        public static CreateTableBuilder<dynamic> TryCreateTable(this MigrationBuilder builder,
            string name,
            Func<ColumnsBuilder, dynamic> columns,
            string? schema = null,
            Action<CreateTableBuilder<dynamic>>? constraints = null,
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
