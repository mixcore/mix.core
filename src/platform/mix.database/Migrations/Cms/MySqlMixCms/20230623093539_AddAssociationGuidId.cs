using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.MySqlMixCms
{
    /// <inheritdoc />
    public partial class AddAssociationGuidId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuidChildId",
                table: "MixDatabaseAssociation",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "GuidParentId",
                table: "MixDatabaseAssociation",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuidChildId",
                table: "MixDatabaseAssociation");

            migrationBuilder.DropColumn(
                name: "GuidParentId",
                table: "MixDatabaseAssociation");
        }
    }
}
