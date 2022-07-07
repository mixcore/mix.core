using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.PostgresqlMixCms
{
    public partial class AddMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MixMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: true),
                    FileName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    FileProperties = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Tags = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    TargetUrl = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false)
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
        }
    }
}
