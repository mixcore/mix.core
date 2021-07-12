using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class add_data_asso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MixDatabaseId",
                table: "MixDataContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "MixDatabaseColumn",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MixDataContentAssociation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWID()"),
                    MixDatabaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "TEXT", nullable: true),
                    ParentType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    DataContentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GuidParentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IntParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDataContentAssociation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixDataContentAssociation");

            migrationBuilder.DropColumn(
                name: "MixDatabaseId",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "MixDatabaseColumn");
        }
    }
}
