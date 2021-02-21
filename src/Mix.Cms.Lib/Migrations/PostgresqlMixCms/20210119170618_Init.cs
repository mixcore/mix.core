using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Mix.Cms.Lib.Migrations.PostgresqlMixCms
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MixService.SetConfig(MixConfigurations.CONST_MIXCORE_VERSION, "1.0.1");
            MixService.SaveSettings();
            migrationBuilder.CreateTable(
                name: "mix_cache",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: false, collation: "und-x-icu"),
                    ExpiredDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_cache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_cms_user",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Address = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Avatar = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    MiddleName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    Username = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_cms_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_culture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Alias = table.Column<string>(type: "varchar(150)", nullable: true, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    FullName = table.Column<string>(type: "varchar(150)", nullable: true, collation: "und-x-icu"),
                    Icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LCID = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_culture", x => x.Id);
                    table.UniqueConstraint("AK_mix_culture_Specificulture", x => x.Specificulture);
                });

            migrationBuilder.CreateTable(
                name: "mix_database",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    FormTemplate = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    EdmTemplate = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    EdmSubject = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    EdmFrom = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    EdmAutoSend = table.Column<bool>(type: "boolean", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_database", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_column",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Configurations = table.Column<string>(type: "text", nullable: true),
                    Regex = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    DefaultValue = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    Options = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    IsRequire = table.Column<bool>(type: "boolean", nullable: false),
                    IsEncrypt = table.Column<bool>(type: "boolean", nullable: false),
                    IsMultiple = table.Column<bool>(type: "boolean", nullable: false),
                    IsSelect = table.Column<bool>(type: "boolean", nullable: false),
                    IsUnique = table.Column<bool>(type: "boolean", nullable: false),
                    ReferenceId = table.Column<int>(type: "integer", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_database_column", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_data_association",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentId = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ParentType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_database_data_association", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_database_data_value",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    MixDatabaseColumnId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseColumnName = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Regex = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    BooleanValue = table.Column<bool>(type: "boolean", nullable: true),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    DateTimeValue = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DoubleValue = table.Column<double>(type: "double precision", nullable: true),
                    IntegerValue = table.Column<int>(type: "integer", nullable: true),
                    StringValue = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    EncryptValue = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    EncryptKey = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    EncryptType = table.Column<int>(type: "integer", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_database_data_value", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    FileName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    FileProperties = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Tags = table.Column<string>(type: "varchar(400)", nullable: true, collation: "und-x-icu"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    TargetUrl = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    TextKeyword = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true, collation: "und-x-icu"),
                    TextDefault = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_theme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Thumbnail = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    PreviewUrl = table.Column<string>(type: "varchar(450)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_configuration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    Keyword = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Category = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_configuration", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Configuration_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_language",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    Keyword = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Category = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    DefaultValue = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_language", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Language_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Fields = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Thumbnail = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Template = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    FormTemplate = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    EdmTemplate = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PostType = table.Column<string>(type: "text", nullable: true),
                    PageSize = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Module_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_page",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    CssClass = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Excerpt = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Layout = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    Level = table.Column<int>(type: "integer", nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    SeoKeywords = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    SeoName = table.Column<string>(type: "varchar(500)", nullable: true, collation: "und-x-icu"),
                    SeoTitle = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    StaticUrl = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Tags = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Template = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    PostType = table.Column<string>(type: "text", nullable: true),
                    Views = table.Column<int>(type: "integer", nullable: true),
                    PageSize = table.Column<int>(type: "integer", nullable: true),
                    ExtraFields = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Page_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    Content = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    PublishedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Excerpt = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Icon = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SeoDescription = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    SeoKeywords = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    SeoName = table.Column<string>(type: "varchar(500)", nullable: true, collation: "und-x-icu"),
                    SeoTitle = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Tags = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Template = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Thumbnail = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Title = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Type = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    Views = table.Column<int>(type: "integer", nullable: true),
                    ExtraFields = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_post", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Post_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_url_alias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    SourceId = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Alias = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_url_alias", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Url_Alias_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_data",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    MixDatabaseId = table.Column<int>(type: "integer", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_database_data", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_database_data_mix_database_MixDatabaseId",
                        column: x => x.MixDatabaseId,
                        principalTable: "mix_database",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page_navigation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PageId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page_navigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_portal_page_navigation_mix_portal_page",
                        column: x => x.PageId,
                        principalTable: "mix_portal_page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_portal_page_navigation_mix_portal_page1",
                        column: x => x.ParentId,
                        principalTable: "mix_portal_page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PageId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_portal_page_role_mix_portal_page",
                        column: x => x.PageId,
                        principalTable: "mix_portal_page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_file",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StringContent = table.Column<string>(type: "text", nullable: false, collation: "und-x-icu"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    FileName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    FolderType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ThemeId = table.Column<int>(type: "integer", nullable: true),
                    ThemeName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_file", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_file_mix_template",
                        column: x => x.ThemeId,
                        principalTable: "mix_theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false, collation: "und-x-icu"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    FileName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    FolderType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    MobileContent = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Scripts = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    SpaContent = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    Styles = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    ThemeId = table.Column<int>(type: "integer", nullable: false),
                    ThemeName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_template_mix_theme",
                        column: x => x.ThemeId,
                        principalTable: "mix_theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    PageId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_module", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Menu_Module_Mix_Module1",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mix_Page_Module_Mix_Page",
                        columns: x => new { x.PageId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_data",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    PageId = table.Column<int>(type: "integer", nullable: true),
                    PostId = table.Column<int>(type: "integer", nullable: true),
                    Fields = table.Column<string>(type: "text", nullable: false, collation: "und-x-icu"),
                    Value = table.Column<string>(type: "text", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_data", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Module_Data_Mix_Module",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mix_module_data_mix_page",
                        columns: x => new { x.PageId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_module_data_mix_post",
                        columns: x => new { x.PostId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_post", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Module_Post_Mix_Module",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Module_Post_Mix_Post",
                        columns: x => new { x.PostId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    PageId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_post", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Page_Post_Mix_Page",
                        columns: x => new { x.PageId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Page_Post_Mix_Post",
                        columns: x => new { x.PostId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_post_association",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    DestinationId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(450)", nullable: true, collation: "und-x-icu"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_post_association", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_post_association_mix_post",
                        columns: x => new { x.SourceId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_post_association_mix_post1",
                        columns: x => new { x.DestinationId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_post_media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    MediaId = table.Column<int>(type: "integer", nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_post_media", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_post_media_mix_media",
                        columns: x => new { x.MediaId, x.Specificulture },
                        principalTable: "mix_media",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_post_media_mix_post",
                        columns: x => new { x.PostId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_post_module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false, collation: "und-x-icu"),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_post_module", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Post_Module_Mix_Module1",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mix_Post_Module_Mix_Post",
                        columns: x => new { x.PostId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "Index_ExpiresAtTime",
                table: "mix_cache",
                column: "ExpiredDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_mix_configuration_Specificulture",
                table: "mix_configuration",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_Mix_Culture",
                table: "mix_culture",
                column: "Specificulture",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_column_MixDatabaseId",
                table: "mix_database_column",
                column: "MixDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_column_ReferenceId",
                table: "mix_database_column",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_data_MixDatabaseId",
                table: "mix_database_data",
                column: "MixDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_data_value_DataId",
                table: "mix_database_data_value",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_file_ThemeId",
                table: "mix_file",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_language_Specificulture",
                table: "mix_language",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_Specificulture",
                table: "mix_module",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_PageId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "PageId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_PageId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "PageId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_PostId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "PostId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_post_ModuleId_Specificulture",
                table: "mix_module_post",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_post_PostId_Specificulture",
                table: "mix_module_post",
                columns: new[] { "PostId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_Specificulture",
                table: "mix_page",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_module_ModuleId_Specificulture",
                table: "mix_page_module",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_module_PageId_Specificulture",
                table: "mix_page_module",
                columns: new[] { "PageId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_post_PageId_Specificulture",
                table: "mix_page_post",
                columns: new[] { "PageId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_post_PostId_Specificulture",
                table: "mix_page_post",
                columns: new[] { "PostId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "FK_mix_portal_page_navigation_mix_portal_page",
                table: "mix_portal_page_navigation",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_portal_page_navigation_ParentId",
                table: "mix_portal_page_navigation",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_portal_page_role_PageId",
                table: "mix_portal_page_role",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_Specificulture",
                table: "mix_post",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_association_DestinationId_Specificulture",
                table: "mix_post_association",
                columns: new[] { "DestinationId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_association_SourceId_Specificulture",
                table: "mix_post_association",
                columns: new[] { "SourceId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_media_MediaId_Specificulture",
                table: "mix_post_media",
                columns: new[] { "MediaId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_media_PostId_Specificulture",
                table: "mix_post_media",
                columns: new[] { "PostId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_module_ModuleId_Specificulture",
                table: "mix_post_module",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_module_PostId_Specificulture",
                table: "mix_post_module",
                columns: new[] { "PostId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_template_file_TemplateId",
                table: "mix_template",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_url_alias_Specificulture",
                table: "mix_url_alias",
                column: "Specificulture");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_cache");

            migrationBuilder.DropTable(
                name: "mix_cms_user");

            migrationBuilder.DropTable(
                name: "mix_configuration");

            migrationBuilder.DropTable(
                name: "mix_database_column");

            migrationBuilder.DropTable(
                name: "mix_database_data");

            migrationBuilder.DropTable(
                name: "mix_database_data_association");

            migrationBuilder.DropTable(
                name: "mix_database_data_value");

            migrationBuilder.DropTable(
                name: "mix_file");

            migrationBuilder.DropTable(
                name: "mix_language");

            migrationBuilder.DropTable(
                name: "mix_module_data");

            migrationBuilder.DropTable(
                name: "mix_module_post");

            migrationBuilder.DropTable(
                name: "mix_page_module");

            migrationBuilder.DropTable(
                name: "mix_page_post");

            migrationBuilder.DropTable(
                name: "mix_portal_page_navigation");

            migrationBuilder.DropTable(
                name: "mix_portal_page_role");

            migrationBuilder.DropTable(
                name: "mix_post_association");

            migrationBuilder.DropTable(
                name: "mix_post_media");

            migrationBuilder.DropTable(
                name: "mix_post_module");

            migrationBuilder.DropTable(
                name: "mix_template");

            migrationBuilder.DropTable(
                name: "mix_url_alias");

            migrationBuilder.DropTable(
                name: "mix_database");

            migrationBuilder.DropTable(
                name: "mix_page");

            migrationBuilder.DropTable(
                name: "mix_portal_page");

            migrationBuilder.DropTable(
                name: "mix_media");

            migrationBuilder.DropTable(
                name: "mix_module");

            migrationBuilder.DropTable(
                name: "mix_post");

            migrationBuilder.DropTable(
                name: "mix_theme");

            migrationBuilder.DropTable(
                name: "mix_culture");
        }
    }
}
