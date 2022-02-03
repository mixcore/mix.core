using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.SqliteMixCms
{
    public partial class UpdateAdditionalDbName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE mix_database SET Name='sys_additional_column_page' WHERE Name='sys_additional_field_page'");
            migrationBuilder.Sql(@"UPDATE mix_database SET Name='sys_additional_column_post' WHERE Name='sys_additional_field_post'");
            migrationBuilder.Sql(@"UPDATE mix_database SET Name='sys_additional_column_module' WHERE Name='sys_additional_field_module'");
            migrationBuilder.Sql(@"UPDATE mix_database SET Name='sys_additional_column' WHERE Name='sys_additional_field'");

            migrationBuilder.Sql(@"UPDATE mix_database_data SET MixDatabaseName='sys_additional_column_page' WHERE MixDatabaseName='sys_additional_field_page'");
            migrationBuilder.Sql(@"UPDATE mix_database_data SET MixDatabaseName='sys_additional_column_post' WHERE MixDatabaseName='sys_additional_field_post'");
            migrationBuilder.Sql(@"UPDATE mix_database_data SET MixDatabaseName='sys_additional_column_module' WHERE MixDatabaseName='sys_additional_field_module'");
            migrationBuilder.Sql(@"UPDATE mix_database_data SET MixDatabaseName='sys_additional_column' WHERE MixDatabaseName='sys_additional_field'");

            migrationBuilder.Sql(@"UPDATE mix_database_data_association SET MixDatabaseName='sys_additional_column_page' WHERE MixDatabaseName='sys_additional_field_page'");
            migrationBuilder.Sql(@"UPDATE mix_database_data_association SET MixDatabaseName='sys_additional_column_post' WHERE MixDatabaseName='sys_additional_field_post'");
            migrationBuilder.Sql(@"UPDATE mix_database_data_association SET MixDatabaseName='sys_additional_column_module' WHERE MixDatabaseName='sys_additional_field_module'");
            migrationBuilder.Sql(@"UPDATE mix_database_data_association SET MixDatabaseName='sys_additional_column' WHERE MixDatabaseName='sys_additional_field'");

            migrationBuilder.Sql(@"UPDATE mix_database_column SET MixDatabaseName='sys_additional_column_page' WHERE MixDatabaseName='sys_additional_field_page'");
            migrationBuilder.Sql(@"UPDATE mix_database_column SET MixDatabaseName='sys_additional_column_post' WHERE MixDatabaseName='sys_additional_field_post'");
            migrationBuilder.Sql(@"UPDATE mix_database_column SET MixDatabaseName='sys_additional_column_module' WHERE MixDatabaseName='sys_additional_field_module'");
            migrationBuilder.Sql(@"UPDATE mix_database_column SET MixDatabaseName='sys_additional_column' WHERE MixDatabaseName='sys_additional_field'");
            
            migrationBuilder.Sql(@"UPDATE mix_database_column SET Name='databaseName' WHERE MixDatabaseName='post_type' AND Name='mix_database_name'");

            migrationBuilder.Sql(@"UPDATE mix_database_data_value SET MixDatabaseName='sys_additional_column_page' WHERE MixDatabaseName='sys_additional_field_page'");
            migrationBuilder.Sql(@"UPDATE mix_database_data_value SET MixDatabaseName='sys_additional_column_post' WHERE MixDatabaseName='sys_additional_field_post'");
            migrationBuilder.Sql(@"UPDATE mix_database_data_value SET MixDatabaseName='sys_additional_column_module' WHERE MixDatabaseName='sys_additional_field_module'");
            migrationBuilder.Sql(@"UPDATE mix_database_data_value SET MixDatabaseName='sys_additional_column' WHERE MixDatabaseName='sys_additional_field'");

            migrationBuilder.Sql(@"UPDATE mix_post SET Type='sys_additional_column_post' WHERE Type='sys_additional_field_post'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
