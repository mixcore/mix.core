using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.SqliteMixCms
{
    public partial class UpdateDbName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("mix_database", "Name", "sys_additional_field_page", "Name", "sys_additional_column_page");
            migrationBuilder.UpdateData("mix_database", "Name", "sys_additional_field_post", "Name", "sys_additional_column_post");
            migrationBuilder.UpdateData("mix_database", "Name", "sys_additional_field_module", "Name", "sys_additional_column_module");
            migrationBuilder.UpdateData("mix_database", "Name", "sys_additional_field", "Name", "sys_additional_column");

            migrationBuilder.UpdateData("mix_database_data", "MixDatabaseName", "sys_additional_field_page", "MixDatabaseName", "sys_additional_column_page");
            migrationBuilder.UpdateData("mix_database_data", "MixDatabaseName", "sys_additional_field_post", "MixDatabaseName", "sys_additional_column_post");
            migrationBuilder.UpdateData("mix_database_data", "MixDatabaseName", "sys_additional_field_module", "MixDatabaseName", "sys_additional_column_module");
            migrationBuilder.UpdateData("mix_database_data", "MixDatabaseName", "sys_additional_field", "MixDatabaseName", "sys_additional_column");

            migrationBuilder.UpdateData("mix_database_column", "MixDatabaseName", "sys_additional_field_page", "MixDatabaseName", "sys_additional_column_page");
            migrationBuilder.UpdateData("mix_database_column", "MixDatabaseName", "sys_additional_field_post", "MixDatabaseName", "sys_additional_column_post");
            migrationBuilder.UpdateData("mix_database_column", "MixDatabaseName", "sys_additional_field_module", "MixDatabaseName", "sys_additional_column_module");
            migrationBuilder.UpdateData("mix_database_column", "MixDatabaseName", "sys_additional_field", "MixDatabaseName", "sys_additional_column");

            migrationBuilder.UpdateData("mix_database_data_value", "MixDatabaseName", "sys_additional_field_page", "MixDatabaseName", "sys_additional_column_page");
            migrationBuilder.UpdateData("mix_database_data_value", "MixDatabaseName", "sys_additional_field_post", "MixDatabaseName", "sys_additional_column_post");
            migrationBuilder.UpdateData("mix_database_data_value", "MixDatabaseName", "sys_additional_field_module", "MixDatabaseName", "sys_additional_column_module");
            migrationBuilder.UpdateData("mix_database_data_value", "MixDatabaseName", "sys_additional_field", "MixDatabaseName", "sys_additional_column");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
