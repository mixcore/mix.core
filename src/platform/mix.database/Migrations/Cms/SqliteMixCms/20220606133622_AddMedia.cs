using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StringValue",
                table: "MixDataContentValue",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.CreateTable(
                name: "MixMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    FileProperties = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    FileType = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    Tags = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    TargetUrl = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixMedia_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixMedia_MixTenantId",
                table: "MixMedia",
                column: "MixTenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixMedia");

            migrationBuilder.AlterColumn<string>(
                name: "StringValue",
                table: "MixDataContentValue",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");
        }
    }
}
