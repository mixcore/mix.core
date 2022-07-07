using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteAccount
{
    public partial class AddUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "RefreshTokens",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "MixUsers",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "MixUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "MixUsers",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "MixUsers",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "MixUsers",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "MixUsers",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "MixUsers");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "MixUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "MixUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "MixUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "MixUsers");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "MixUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "RefreshTokens",
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
