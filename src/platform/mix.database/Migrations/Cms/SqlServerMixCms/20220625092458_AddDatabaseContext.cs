using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    public partial class AddDatabaseContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MixDatabaseContextDatabaseAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    LeftId = table.Column<int>(type: "int", nullable: false),
                    RightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseContextDatabaseAssociation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixDatabaseContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatabaseProvider = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ConnectionString = table.Column<string>(type: "varchar(250)", nullable: false),
                    MixDatabaseContextDatabaseAssociationId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    SystemName = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseContext", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDatabaseContext_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                        column: x => x.MixDatabaseContextDatabaseAssociationId,
                        principalTable: "MixDatabaseContextDatabaseAssociation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixDatabaseContext_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseContext_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseContext_MixTenantId",
                table: "MixDatabaseContext",
                column: "MixTenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId",
                principalTable: "MixDatabaseContextDatabaseAssociation",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");

            migrationBuilder.DropTable(
                name: "MixDatabaseContext");

            migrationBuilder.DropTable(
                name: "MixDatabaseContextDatabaseAssociation");

            migrationBuilder.DropIndex(
                name: "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");

            migrationBuilder.DropColumn(
                name: "MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");
        }
    }
}
