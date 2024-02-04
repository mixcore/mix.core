using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.MySqlMixCms
{
    /// <inheritdoc />
    public partial class AddDbContextNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NamingConvention",
                table: "MixDatabaseContext",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NamingConvention",
                table: "MixDatabaseContext");
        }
    }
}
