using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class updTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixDataContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixDataContent",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");
        }
    }
}
