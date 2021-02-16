using Microsoft.EntityFrameworkCore.Migrations;
using Mix.Cms.Lib.Models.Cms;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Mix.Cms.Lib.Migrations.MySqlMixCms
{
    public partial class RenameDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = new MixCmsContext())
            {
                if (TableExists("mix_attribute_set"))
                {
                    migrationBuilder.DropTable("mix_related_attribute_set");
                    migrationBuilder.DropTable("mix_attribute_set_reference");

                    migrationBuilder.RenameTable("mix_related_post", null, "mix_post_association");
                    migrationBuilder.RenameTable("mix_attribute_set", null, "mix_database");
                    migrationBuilder.RenameTable("mix_related_attribute_data", null, "mix_database_data_association");
                    migrationBuilder.RenameTable("mix_attribute_set_value", null, "mix_database_data_value");
                    migrationBuilder.RenameTable("mix_attribute_set_data", null, "mix_database_data");

                    migrationBuilder.RenameColumn("AttributeFieldId", "mix_database_data_value", "MixDatabaseColumnId");
                    migrationBuilder.RenameColumn("AttributeFieldName", "mix_database_data_value", "MixDatabaseColumnName");
                    migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data_value", "MixDatabaseName");


                    migrationBuilder.RenameColumn("AttributeSetId", "mix_database_data_association", "MixDatabaseId");
                    migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data_association", "MixDatabaseName");

                    migrationBuilder.RenameColumn("AttributeSetId", "mix_database_data", "MixDatabaseId");
                    migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data", "MixDatabaseName");

                    migrationBuilder.RenameTable("mix_attribute_field", null, "mix_database_column");
                    migrationBuilder.RenameColumn("AttributeSetId", "mix_database_column", "MixDatabaseId");
                    migrationBuilder.RenameColumn("AttributeSetName", "mix_database_column", "MixDatabaseName");
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        private bool TableExists(string tableName, string schema = "dbo")
        {
            using (var context = new MixCmsContext())
            {
                var connection = context.Database.GetDbConnection();

                if (connection.State.Equals(ConnectionState.Closed))
                    connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
            SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_SCHEMA = @Schema
            AND TABLE_NAME = @TableName";

                    var schemaParam = command.CreateParameter();
                    schemaParam.ParameterName = "@Schema";
                    schemaParam.Value = schema;
                    command.Parameters.Add(schemaParam);

                    var tableNameParam = command.CreateParameter();
                    tableNameParam.ParameterName = "@TableName";
                    tableNameParam.Value = tableName;
                    command.Parameters.Add(tableNameParam);

                    return command.ExecuteScalar() != null;
                }
            }
        }
    }
}
