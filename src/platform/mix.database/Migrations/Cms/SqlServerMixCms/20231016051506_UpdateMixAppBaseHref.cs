using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    /// <inheritdoc />
    public partial class UpdateMixAppBaseHref : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseRoute",
                table: "MixApplication",
                newName: "DeployUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeployUrl",
                table: "MixApplication",
                newName: "BaseRoute");
        }
    }
}
