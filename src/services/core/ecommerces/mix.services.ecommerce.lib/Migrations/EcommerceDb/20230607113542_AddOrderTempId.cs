using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Services.Ecommerce.Lib.Migrations.EcommerceDb
{
    /// <inheritdoc />
    public partial class AddOrderTempId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Warehouse",
                newName: "Warehouse");

            migrationBuilder.AddColumn<Guid>(
                name: "TempId",
                table: "OrderDetail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempId",
                table: "OrderDetail");

            migrationBuilder.RenameTable(
                name: "Warehouse",
                newName: "ProductVariant");
        }
    }
}
