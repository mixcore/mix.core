using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mix.Database.Migrations.PostgresqlMixCms
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "MixTenant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixTenant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixConfiguration_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixCulture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Alias = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Icon = table.Column<string>(type: "varchar(4000)", nullable: true),
                    Lcid = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixCulture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixCulture_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDatabase_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixDomain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDomain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDomain_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixLanguage_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixModule_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixPost_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixUrlAlias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceContentId = table.Column<int>(type: "integer", nullable: true),
                    SourceContentGuidId = table.Column<Guid>(type: "uuid", nullable: true),
                    Alias = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUrlAlias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixUrlAlias_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixConfigurationContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefaultContent = table.Column<string>(type: "varchar(4000)", nullable: false, collation: "und-x-icu"),
                    MixConfigurationId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixConfigurationContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixConfigurationContent_MixConfiguration_MixConfigurationId",
                        column: x => x.MixConfigurationId,
                        principalTable: "MixConfiguration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixConfigurationContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixDataContentAssociation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "text", nullable: true),
                    ParentType = table.Column<string>(type: "varchar(50)", nullable: false),
                    DataContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    GuidParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    IntParentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDataContentAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDataContentAssociation_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixData_MixDatabase_MixDatabaseId",
                        column: x => x.MixDatabaseId,
                        principalTable: "MixDatabase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixDatabaseColumn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false),
                    Configurations = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    ReferenceId = table.Column<int>(type: "integer", nullable: true),
                    DefaultValue = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseColumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDatabaseColumn_MixDatabase_MixDatabaseId",
                        column: x => x.MixDatabaseId,
                        principalTable: "MixDatabase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixLanguageContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefaultContent = table.Column<string>(type: "varchar(4000)", nullable: false, collation: "und-x-icu"),
                    MixLanguageId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixLanguageContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixLanguageContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixLanguageContent_MixLanguage_MixLanguageId",
                        column: x => x.MixLanguageId,
                        principalTable: "MixLanguage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixDataContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "text", nullable: true),
                    MixDataId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    LayoutId = table.Column<int>(type: "int", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoDescription = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoKeywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    PublishedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDataContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDataContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixDataContent_MixData_MixDataId",
                        column: x => x.MixDataId,
                        principalTable: "MixData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixDataContentValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    MixDatabaseColumnName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false),
                    BooleanValue = table.Column<bool>(type: "boolean", nullable: true),
                    DateTimeValue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DoubleValue = table.Column<double>(type: "double precision", nullable: true),
                    IntegerValue = table.Column<int>(type: "integer", nullable: true),
                    StringValue = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    EncryptValue = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    EncryptKey = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    EncryptType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    MixDatabaseColumnId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    MixDataContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDataContentValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDataContentValue_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixDataContentValue_MixDatabaseColumn_MixDatabaseColumnId",
                        column: x => x.MixDatabaseColumnId,
                        principalTable: "MixDatabaseColumn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixDataContentValue_MixDataContent_MixDataContentId",
                        column: x => x.MixDataContentId,
                        principalTable: "MixDataContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixModuleContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: true),
                    ClassName = table.Column<string>(type: "varchar(50)", nullable: true),
                    PageSize = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    SimpleDataColumns = table.Column<string>(type: "text", nullable: true),
                    MixModuleId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    LayoutId = table.Column<int>(type: "int", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoDescription = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoKeywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    PublishedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MixDatabaseName = table.Column<string>(type: "text", nullable: true),
                    MixDataContentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixModuleContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixModuleContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixModuleContent_MixDataContent_MixDataContentId",
                        column: x => x.MixDataContentId,
                        principalTable: "MixDataContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixModuleContent_MixModule_MixModuleId",
                        column: x => x.MixModuleId,
                        principalTable: "MixModule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixPostContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassName = table.Column<string>(type: "varchar(50)", nullable: true),
                    MixPostId = table.Column<int>(type: "integer", nullable: true),
                    MixPostContentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    LayoutId = table.Column<int>(type: "int", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoDescription = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoKeywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    PublishedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MixDatabaseName = table.Column<string>(type: "text", nullable: true),
                    MixDataContentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixPostContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixPostContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixPostContent_MixDataContent_MixDataContentId",
                        column: x => x.MixDataContentId,
                        principalTable: "MixDataContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixPostContent_MixPost_MixPostId",
                        column: x => x.MixPostId,
                        principalTable: "MixPost",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixPostContent_MixPostContent_MixPostContentId",
                        column: x => x.MixPostContentId,
                        principalTable: "MixPostContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixTheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PreviewUrl = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    MixDatabaseName = table.Column<string>(type: "text", nullable: true),
                    MixDataContentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false),
                    SystemName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixTheme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixTheme_MixDataContent_MixDataContentId",
                        column: x => x.MixDataContentId,
                        principalTable: "MixDataContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixTheme_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixModuleData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SimpleDataColumns = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ModuleContentId = table.Column<int>(type: "integer", nullable: false),
                    MixModuleContentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Excerpt = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    LayoutId = table.Column<int>(type: "integer", nullable: true),
                    TemplateId = table.Column<int>(type: "integer", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "text", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true),
                    SeoKeywords = table.Column<string>(type: "text", nullable: true),
                    SeoName = table.Column<string>(type: "text", nullable: true),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    PublishedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixModuleData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixModuleData_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixModuleData_MixModuleContent_MixModuleContentId",
                        column: x => x.MixModuleContentId,
                        principalTable: "MixModuleContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixModulePostAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MixModuleContentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LeftId = table.Column<int>(type: "integer", nullable: false),
                    RightId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixModulePostAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixModulePostAssociation_MixModuleContent_MixModuleContentId",
                        column: x => x.MixModuleContentId,
                        principalTable: "MixModuleContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixPage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MixPostContentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixPage_MixPostContent_MixPostContentId",
                        column: x => x.MixPostContentId,
                        principalTable: "MixPostContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixPage_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixViewTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    FileName = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    FolderType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Scripts = table.Column<string>(type: "text", nullable: false, collation: "und-x-icu"),
                    Styles = table.Column<string>(type: "text", nullable: false, collation: "und-x-icu"),
                    MixThemeName = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    MixThemeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixViewTemplate_MixTheme_MixThemeId",
                        column: x => x.MixThemeId,
                        principalTable: "MixTheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixPageContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassName = table.Column<string>(type: "varchar(50)", nullable: true),
                    PageSize = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    MixPageId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    MixCultureId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    LayoutId = table.Column<int>(type: "int", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoDescription = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoKeywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    SeoName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoTitle = table.Column<string>(type: "text", nullable: true),
                    PublishedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MixDatabaseName = table.Column<string>(type: "text", nullable: true),
                    MixDataContentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixPageContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixPageContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixPageContent_MixDataContent_MixDataContentId",
                        column: x => x.MixDataContentId,
                        principalTable: "MixDataContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixPageContent_MixPage_MixPageId",
                        column: x => x.MixPageId,
                        principalTable: "MixPage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixPageModuleAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MixPageContentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LeftId = table.Column<int>(type: "integer", nullable: false),
                    RightId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixPageModuleAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixPageModuleAssociation_MixPageContent_MixPageContentId",
                        column: x => x.MixPageContentId,
                        principalTable: "MixPageContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixPagePostAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MixPageContentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LeftId = table.Column<int>(type: "integer", nullable: false),
                    RightId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixPagePostAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixPagePostAssociation_MixPageContent_MixPageContentId",
                        column: x => x.MixPageContentId,
                        principalTable: "MixPageContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixConfiguration_MixTenantId",
                table: "MixConfiguration",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixConfigurationContent_MixConfigurationId",
                table: "MixConfigurationContent",
                column: "MixConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_MixConfigurationContent_MixCultureId",
                table: "MixConfigurationContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixCulture_MixTenantId",
                table: "MixCulture",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixData_MixDatabaseId",
                table: "MixData",
                column: "MixDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabase_MixTenantId",
                table: "MixDatabase",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseColumn_MixDatabaseId",
                table: "MixDatabaseColumn",
                column: "MixDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContent_MixCultureId",
                table: "MixDataContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContent_MixDataId",
                table: "MixDataContent",
                column: "MixDataId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentAssociation_MixCultureId",
                table: "MixDataContentAssociation",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentValue_MixCultureId",
                table: "MixDataContentValue",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentValue_MixDatabaseColumnId",
                table: "MixDataContentValue",
                column: "MixDatabaseColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentValue_MixDataContentId",
                table: "MixDataContentValue",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDomain_MixTenantId",
                table: "MixDomain",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixLanguage_MixTenantId",
                table: "MixLanguage",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixLanguageContent_MixCultureId",
                table: "MixLanguageContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixLanguageContent_MixLanguageId",
                table: "MixLanguageContent",
                column: "MixLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModule_MixTenantId",
                table: "MixModule",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleContent_MixCultureId",
                table: "MixModuleContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleContent_MixDataContentId",
                table: "MixModuleContent",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleContent_MixModuleId",
                table: "MixModuleContent",
                column: "MixModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleData_MixCultureId",
                table: "MixModuleData",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleData_MixModuleContentId",
                table: "MixModuleData",
                column: "MixModuleContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModulePostAssociation_MixModuleContentId",
                table: "MixModulePostAssociation",
                column: "MixModuleContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPage_MixPostContentId",
                table: "MixPage",
                column: "MixPostContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPage_MixTenantId",
                table: "MixPage",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPageContent_MixCultureId",
                table: "MixPageContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPageContent_MixDataContentId",
                table: "MixPageContent",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPageContent_MixPageId",
                table: "MixPageContent",
                column: "MixPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPageModuleAssociation_MixPageContentId",
                table: "MixPageModuleAssociation",
                column: "MixPageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPagePostAssociation_MixPageContentId",
                table: "MixPagePostAssociation",
                column: "MixPageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPost_MixTenantId",
                table: "MixPost",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPostContent_MixCultureId",
                table: "MixPostContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPostContent_MixDataContentId",
                table: "MixPostContent",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPostContent_MixPostContentId",
                table: "MixPostContent",
                column: "MixPostContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPostContent_MixPostId",
                table: "MixPostContent",
                column: "MixPostId");

            migrationBuilder.CreateIndex(
                name: "IX_MixTheme_MixDataContentId",
                table: "MixTheme",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixTheme_MixTenantId",
                table: "MixTheme",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixUrlAlias_MixTenantId",
                table: "MixUrlAlias",
                column: "MixTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MixViewTemplate_MixThemeId",
                table: "MixViewTemplate",
                column: "MixThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixConfigurationContent");

            migrationBuilder.DropTable(
                name: "MixDataContentAssociation");

            migrationBuilder.DropTable(
                name: "MixDataContentValue");

            migrationBuilder.DropTable(
                name: "MixDomain");

            migrationBuilder.DropTable(
                name: "MixLanguageContent");

            migrationBuilder.DropTable(
                name: "MixModuleData");

            migrationBuilder.DropTable(
                name: "MixModulePostAssociation");

            migrationBuilder.DropTable(
                name: "MixPageModuleAssociation");

            migrationBuilder.DropTable(
                name: "MixPagePostAssociation");

            migrationBuilder.DropTable(
                name: "MixUrlAlias");

            migrationBuilder.DropTable(
                name: "MixViewTemplate");

            migrationBuilder.DropTable(
                name: "MixConfiguration");

            migrationBuilder.DropTable(
                name: "MixDatabaseColumn");

            migrationBuilder.DropTable(
                name: "MixLanguage");

            migrationBuilder.DropTable(
                name: "MixModuleContent");

            migrationBuilder.DropTable(
                name: "MixPageContent");

            migrationBuilder.DropTable(
                name: "MixTheme");

            migrationBuilder.DropTable(
                name: "MixModule");

            migrationBuilder.DropTable(
                name: "MixPage");

            migrationBuilder.DropTable(
                name: "MixPostContent");

            migrationBuilder.DropTable(
                name: "MixDataContent");

            migrationBuilder.DropTable(
                name: "MixPost");

            migrationBuilder.DropTable(
                name: "MixCulture");

            migrationBuilder.DropTable(
                name: "MixData");

            migrationBuilder.DropTable(
                name: "MixDatabase");

            migrationBuilder.DropTable(
                name: "MixTenant");
        }
    }
}
