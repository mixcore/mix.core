using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.MySqlMixCms
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_attribute_set",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FormTemplate = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EdmTemplate = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EdmSubject = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EdmFrom = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EdmAutoSend = table.Column<ulong>(type: "bit(1)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_set", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_attribute_set_value",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    AttributeFieldId = table.Column<int>(nullable: false),
                    AttributeFieldName = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    AttributeSetName = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Regex = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    BooleanValue = table.Column<ulong>(type: "bit(1)", nullable: true),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DateTimeValue = table.Column<DateTime>(type: "datetime", nullable: true),
                    DoubleValue = table.Column<double>(nullable: true),
                    IntegerValue = table.Column<int>(nullable: true),
                    StringValue = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EncryptValue = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EncryptKey = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EncryptType = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_set_value", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_cache",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Value = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ExpiredDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_cache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_cms_user",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Address = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Avatar = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    MiddleName = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Username = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_cms_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_culture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FullName = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Icon = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LCID = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_culture", x => x.Id);
                    table.UniqueConstraint("AK_mix_culture_Specificulture", x => x.Specificulture);
                });

            migrationBuilder.CreateTable(
                name: "mix_media",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileName = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileProperties = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileSize = table.Column<long>(nullable: false),
                    FileType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Title = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Tags = table.Column<string>(type: "varchar(400)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    TargetUrl = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Icon = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    TextKeyword = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Url = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    TextDefault = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Level = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_related_attribute_data",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ParentId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ParentType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    AttributeSetId = table.Column<int>(nullable: false),
                    AttributeSetName = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_related_data",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Type = table.Column<int>(nullable: false),
                    ParentId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ParentType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    AttributeSetId = table.Column<int>(nullable: false),
                    AttributeSetName = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_theme",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Thumbnail = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    PreviewUrl = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_attribute_field",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttributeSetId = table.Column<int>(nullable: false),
                    AttributeSetName = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Configurations = table.Column<string>(nullable: true),
                    Regex = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DefaultValue = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Options = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    IsRequire = table.Column<ulong>(type: "bit(1)", nullable: false),
                    IsEncrypt = table.Column<ulong>(type: "bit(1)", nullable: false),
                    IsMultiple = table.Column<ulong>(type: "bit(1)", nullable: false),
                    IsSelect = table.Column<ulong>(type: "bit(1)", nullable: false),
                    IsUnique = table.Column<ulong>(type: "bit(1)", nullable: false),
                    ReferenceId = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_field", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_attribute_field_mix_attribute_set",
                        column: x => x.AttributeSetId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_attribute_field_mix_attribute_set1",
                        column: x => x.ReferenceId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_attribute_set_data",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    AttributeSetId = table.Column<int>(nullable: false),
                    AttributeSetName = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_attribute_set_data_mix_attribute_set",
                        column: x => x.AttributeSetId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_attribute_set_reference",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    ParentType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    AttributeSetId = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_set_reference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_attribute_set_reference_mix_attribute_set",
                        column: x => x.AttributeSetId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_related_attribute_set",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    AttributeSetId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    ParentType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_related_attribute_set_mix_attribute_set",
                        column: x => x.Id,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_configuration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Keyword = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Category = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Value = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Keyword = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Category = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Value = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    DefaultValue = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Fields = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Thumbnail = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Template = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FormTemplate = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    EdmTemplate = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Type = table.Column<int>(nullable: false),
                    PostType = table.Column<string>(nullable: true),
                    PageSize = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Content = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CssClass = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Excerpt = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Icon = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Layout = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Level = table.Column<int>(nullable: true),
                    SeoDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SeoKeywords = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SeoName = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SeoTitle = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    StaticUrl = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Tags = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Template = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Title = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Type = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    PostType = table.Column<string>(nullable: true),
                    Views = table.Column<int>(nullable: true),
                    PageSize = table.Column<int>(nullable: true),
                    ExtraFields = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Content = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    PublishedDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Excerpt = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Icon = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SeoDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SeoKeywords = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SeoName = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SeoTitle = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Tags = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Template = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Thumbnail = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Title = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Type = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Views = table.Column<int>(nullable: true),
                    ExtraFields = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SourceId = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Alias = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Url_Alias_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page_navigation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PageId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
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
                    PageId = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.RoleId, x.PageId });
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StringContent = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileName = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FolderType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ThemeId = table.Column<int>(nullable: true),
                    ThemeName = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FileName = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    FolderType = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    MobileContent = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Scripts = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SpaContent = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Styles = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ThemeId = table.Column<int>(nullable: false),
                    ThemeName = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ModuleId = table.Column<int>(nullable: false),
                    PageId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Position = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ModuleId = table.Column<int>(nullable: false),
                    PageId = table.Column<int>(nullable: true),
                    PostId = table.Column<int>(nullable: true),
                    Fields = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Value = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    PostId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    PostId = table.Column<int>(nullable: false),
                    PageId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                name: "mix_post_media",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    MediaId = table.Column<int>(nullable: false),
                    PostId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Position = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    ModuleId = table.Column<int>(nullable: false),
                    PostId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Position = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
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

            migrationBuilder.CreateTable(
                name: "mix_related_post",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    SourceId = table.Column<int>(nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    Image = table.Column<string>(type: "varchar(450)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci"),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8")
                        .Annotation("MySql:Collation", "utf8_unicode_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_related_post_mix_post1",
                        columns: x => new { x.DestinationId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_related_post_mix_post",
                        columns: x => new { x.SourceId, x.Specificulture },
                        principalTable: "mix_post",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_field_AttributeSetId",
                table: "mix_attribute_field",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_field_ReferenceId",
                table: "mix_attribute_field",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_data_AttributeSetId",
                table: "mix_attribute_set_data",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_reference_AttributeSetId",
                table: "mix_attribute_set_reference",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_value_DataId",
                table: "mix_attribute_set_value",
                column: "DataId");

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
                name: "IX_mix_module_data_ModuleId_PageId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "PageId", "Specificulture" });

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
                name: "IX_mix_related_post_DestinationId_Specificulture",
                table: "mix_related_post",
                columns: new[] { "DestinationId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_post_SourceId_Specificulture",
                table: "mix_related_post",
                columns: new[] { "SourceId", "Specificulture" });

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
                name: "mix_attribute_field");

            migrationBuilder.DropTable(
                name: "mix_attribute_set_data");

            migrationBuilder.DropTable(
                name: "mix_attribute_set_reference");

            migrationBuilder.DropTable(
                name: "mix_attribute_set_value");

            migrationBuilder.DropTable(
                name: "mix_cache");

            migrationBuilder.DropTable(
                name: "mix_cms_user");

            migrationBuilder.DropTable(
                name: "mix_configuration");

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
                name: "mix_post_media");

            migrationBuilder.DropTable(
                name: "mix_post_module");

            migrationBuilder.DropTable(
                name: "mix_related_attribute_data");

            migrationBuilder.DropTable(
                name: "mix_related_attribute_set");

            migrationBuilder.DropTable(
                name: "mix_related_data");

            migrationBuilder.DropTable(
                name: "mix_related_post");

            migrationBuilder.DropTable(
                name: "mix_template");

            migrationBuilder.DropTable(
                name: "mix_url_alias");

            migrationBuilder.DropTable(
                name: "mix_page");

            migrationBuilder.DropTable(
                name: "mix_portal_page");

            migrationBuilder.DropTable(
                name: "mix_media");

            migrationBuilder.DropTable(
                name: "mix_module");

            migrationBuilder.DropTable(
                name: "mix_attribute_set");

            migrationBuilder.DropTable(
                name: "mix_post");

            migrationBuilder.DropTable(
                name: "mix_theme");

            migrationBuilder.DropTable(
                name: "mix_culture");
        }
    }
}
