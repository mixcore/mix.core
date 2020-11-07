using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.MySqlMixCms
{
    public partial class Add_Field_Configurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Configurations",
                table: "mix_attribute_field",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Configurations",
                table: "mix_attribute_field");
        }
    }
}
