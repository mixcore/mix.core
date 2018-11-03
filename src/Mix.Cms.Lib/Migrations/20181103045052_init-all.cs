using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class initall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_cms_user",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Address = table.Column<string>(maxLength: 450, nullable: true),
                    Avatar = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Username = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_cms_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_copy",
                columns: table => new
                {
                    Culture = table.Column<string>(maxLength: 10, nullable: false),
                    Keyword = table.Column<string>(maxLength: 250, nullable: false),
                    Note = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_copy", x => new { x.Culture, x.Keyword });
                });

            migrationBuilder.CreateTable(
                name: "mix_culture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(maxLength: 150, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    FullName = table.Column<string>(maxLength: 150, nullable: true),
                    Icon = table.Column<string>(maxLength: 50, nullable: true),
                    LCID = table.Column<string>(maxLength: 50, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_culture", x => x.Id);
                    table.UniqueConstraint("AK_mix_culture_Specificulture", x => x.Specificulture);
                });

            migrationBuilder.CreateTable(
                name: "mix_customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 256, nullable: true),
                    Username = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    FullName = table.Column<string>(maxLength: 250, nullable: true),
                    BirthDay = table.Column<DateTime>(type: "datetime", nullable: true),
                    Avatar = table.Column<string>(maxLength: 250, nullable: true),
                    Address = table.Column<string>(maxLength: 450, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    IsAgreeNotified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_media",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    Extension = table.Column<string>(maxLength: 50, nullable: false),
                    FileFolder = table.Column<string>(maxLength: 250, nullable: false),
                    FileName = table.Column<string>(maxLength: 250, nullable: false),
                    FileProperties = table.Column<string>(maxLength: 4000, nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileType = table.Column<string>(maxLength: 50, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Title = table.Column<string>(maxLength: 4000, nullable: true),
                    Tags = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_media", x => new { x.Id, x.Specificulture });
                });

            migrationBuilder.CreateTable(
                name: "mix_parameter",
                columns: table => new
                {
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_parameter", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(maxLength: 50, nullable: true),
                    TextKeyword = table.Column<string>(maxLength: 250, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Url = table.Column<string>(maxLength: 250, nullable: true),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    TextDefault = table.Column<string>(maxLength: 250, nullable: true),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_position",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 250, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_set_attribute",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 350, nullable: true),
                    Fields = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_set_attribute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_theme",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    PreviewUrl = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_configuration",
                columns: table => new
                {
                    Keyword = table.Column<string>(maxLength: 250, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Category = table.Column<string>(maxLength: 250, nullable: true),
                    DataType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Value = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_configuration", x => new { x.Keyword, x.Specificulture });
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
                    Keyword = table.Column<string>(maxLength: 250, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Category = table.Column<string>(maxLength: 250, nullable: true),
                    DataType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Value = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_language", x => new { x.Keyword, x.Specificulture });
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
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    Fields = table.Column<string>(maxLength: 4000, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Template = table.Column<string>(maxLength: 250, nullable: true),
                    FormTemplate = table.Column<string>(maxLength: 4000, nullable: true),
                    Title = table.Column<string>(maxLength: 250, nullable: true),
                    Type = table.Column<int>(nullable: false, defaultValueSql: "('0')"),
                    PageSize = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
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
                name: "mix_url_alias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    SourceId = table.Column<string>(maxLength: 250, nullable: true),
                    Type = table.Column<int>(nullable: false, defaultValueSql: "('0')"),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Alias = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
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
                name: "mix_order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_order", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_order_mix_cms_customer",
                        column: x => x.CustomerId,
                        principalTable: "mix_customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page_navigation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page_navigation", x => new { x.Id, x.ParentId });
                    table.ForeignKey(
                        name: "FK_mix_portal_page_navigation_mix_portal_page",
                        column: x => x.Id,
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
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    PageId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page_role", x => new { x.RoleId, x.PageId });
                    table.ForeignKey(
                        name: "FK_mix_portal_page_role_mix_portal_page",
                        column: x => x.PageId,
                        principalTable: "mix_portal_page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_portal_page_position",
                columns: table => new
                {
                    PositionId = table.Column<int>(nullable: false),
                    PortalPageId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_portal_page_position", x => new { x.PositionId, x.PortalPageId });
                    table.ForeignKey(
                        name: "FK_Mix_PortalPage_Position_Mix_PortalPage",
                        column: x => x.PortalPageId,
                        principalTable: "mix_portal_page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_PortalPage_Position_Mix_Position",
                        column: x => x.PositionId,
                        principalTable: "mix_position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    SetAttributeId = table.Column<int>(nullable: true),
                    SetAttributeData = table.Column<string>(type: "ntext", nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Excerpt = table.Column<string>(nullable: true),
                    ExtraProperties = table.Column<string>(type: "ntext", nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    SeoDescription = table.Column<string>(maxLength: 4000, nullable: true),
                    SeoKeywords = table.Column<string>(maxLength: 4000, nullable: true),
                    SeoName = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    SeoTitle = table.Column<string>(maxLength: 4000, nullable: true),
                    Source = table.Column<string>(maxLength: 250, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Tags = table.Column<string>(maxLength: 500, nullable: true),
                    Template = table.Column<string>(maxLength: 250, nullable: true),
                    Thumbnail = table.Column<string>(maxLength: 250, nullable: true),
                    Title = table.Column<string>(maxLength: 4000, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Views = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_article", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_article_mix_set_attribute",
                        column: x => x.SetAttributeId,
                        principalTable: "mix_set_attribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Article_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    SetAttributeId = table.Column<int>(nullable: true),
                    SetAttributeData = table.Column<string>(type: "ntext", nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CssClass = table.Column<string>(maxLength: 50, nullable: true),
                    Excerpt = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(maxLength: 50, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Layout = table.Column<string>(maxLength: 50, nullable: true),
                    Level = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    SeoDescription = table.Column<string>(maxLength: 4000, nullable: true),
                    SeoKeywords = table.Column<string>(maxLength: 4000, nullable: true),
                    SeoName = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    SeoTitle = table.Column<string>(maxLength: 4000, nullable: true),
                    StaticUrl = table.Column<string>(maxLength: 250, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Tags = table.Column<string>(maxLength: 500, nullable: true),
                    Template = table.Column<string>(maxLength: 250, nullable: true),
                    Title = table.Column<string>(maxLength: 4000, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Views = table.Column<int>(nullable: true),
                    PageSize = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_page_mix_set_attribute",
                        column: x => x.SetAttributeId,
                        principalTable: "mix_set_attribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Page_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    SetAttributeId = table.Column<int>(nullable: true),
                    SetAttributeData = table.Column<string>(type: "ntext", nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Excerpt = table.Column<string>(nullable: true),
                    ExtraProperties = table.Column<string>(type: "ntext", nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Price = table.Column<double>(nullable: false),
                    PrivacyId = table.Column<string>(maxLength: 10, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    SeoDescription = table.Column<string>(maxLength: 4000, nullable: true),
                    SeoKeywords = table.Column<string>(maxLength: 4000, nullable: true),
                    SeoName = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    SeoTitle = table.Column<string>(maxLength: 4000, nullable: true),
                    Source = table.Column<string>(maxLength: 250, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Tags = table.Column<string>(maxLength: 500, nullable: true),
                    Template = table.Column<string>(maxLength: 250, nullable: true),
                    Thumbnail = table.Column<string>(maxLength: 250, nullable: true),
                    Title = table.Column<string>(maxLength: 4000, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Views = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true, defaultValueSql: "(N'')"),
                    DealPrice = table.Column<double>(nullable: true, defaultValueSql: "((0))"),
                    Discount = table.Column<double>(nullable: false),
                    ImportPrice = table.Column<double>(nullable: false),
                    Material = table.Column<string>(maxLength: 250, nullable: true),
                    NormalPrice = table.Column<double>(nullable: false),
                    PackageCount = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    TotalSaled = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_product", x => new { x.Id, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_product_mix_set_attribute",
                        column: x => x.SetAttributeId,
                        principalTable: "mix_set_attribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Product_Mix_Culture",
                        column: x => x.Specificulture,
                        principalTable: "mix_culture",
                        principalColumn: "Specificulture",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_file",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "ntext", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Extension = table.Column<string>(maxLength: 50, nullable: false),
                    FileFolder = table.Column<string>(maxLength: 250, nullable: false),
                    FileName = table.Column<string>(maxLength: 250, nullable: false),
                    FolderType = table.Column<string>(maxLength: 50, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    ThemeId = table.Column<int>(nullable: true),
                    ThemeName = table.Column<string>(maxLength: 250, nullable: false)
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
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "ntext", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Extension = table.Column<string>(maxLength: 50, nullable: false),
                    FileFolder = table.Column<string>(maxLength: 250, nullable: false),
                    FileName = table.Column<string>(maxLength: 250, nullable: false),
                    FolderType = table.Column<string>(maxLength: 50, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MobileContent = table.Column<string>(type: "ntext", nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Scripts = table.Column<string>(type: "ntext", nullable: true),
                    SpaContent = table.Column<string>(type: "ntext", nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Styles = table.Column<string>(type: "ntext", nullable: true),
                    ThemeId = table.Column<int>(nullable: false),
                    ThemeName = table.Column<string>(maxLength: 250, nullable: false)
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
                name: "mix_article_media",
                columns: table => new
                {
                    MediaId = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_article_media", x => new { x.MediaId, x.ArticleId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_article_media_mix_article",
                        columns: x => new { x.ArticleId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_article_media_mix_media",
                        columns: x => new { x.MediaId, x.Specificulture },
                        principalTable: "mix_media",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_article_module",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_article_module", x => new { x.ModuleId, x.ArticleId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Article_Module_Mix_Article",
                        columns: x => new { x.ArticleId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Article_Module_Mix_Module1",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_article",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_article", x => new { x.ArticleId, x.ModuleId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Module_Article_Mix_Article",
                        columns: x => new { x.ArticleId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Module_Article_Mix_Module",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_related_article",
                columns: table => new
                {
                    SourceId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    Image = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_related_article", x => new { x.SourceId, x.DestinationId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_related_article_mix_article1",
                        columns: x => new { x.DestinationId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_related_article_mix_article",
                        columns: x => new { x.SourceId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_article",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: true, defaultValueSql: "((0))"),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_article", x => new { x.ArticleId, x.CategoryId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Page_Article_Mix_Article",
                        columns: x => new { x.ArticleId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Page_Article_Mix_Page",
                        columns: x => new { x.CategoryId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_module",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_module", x => new { x.ModuleId, x.CategoryId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Page_Module_Mix_Page",
                        columns: x => new { x.CategoryId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Menu_Module_Mix_Module1",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_page",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_page", x => new { x.Id, x.ParentId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Page_Page_Mix_Page",
                        columns: x => new { x.Id, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Page_Page_Mix_Page1",
                        columns: x => new { x.ParentId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_position",
                columns: table => new
                {
                    PositionId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_position", x => new { x.PositionId, x.CategoryId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Page_Position_Mix_Position",
                        column: x => x.PositionId,
                        principalTable: "mix_position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mix_Page_Position_Mix_Page",
                        columns: x => new { x.CategoryId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: true),
                    ArticleId = table.Column<int>(nullable: true),
                    OrderId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    FullName = table.Column<string>(maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsReviewed = table.Column<bool>(nullable: true),
                    IsVisible = table.Column<bool>(nullable: true),
                    Rating = table.Column<double>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    UpdatedBy = table.Column<string>(maxLength: 250, nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_comment_mix_article",
                        columns: x => new { x.ArticleId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_comment_mix_order",
                        columns: x => new { x.OrderId, x.Specificulture },
                        principalTable: "mix_order",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_comment_mix_product",
                        columns: x => new { x.ProductId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_product",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_product", x => new { x.ProductId, x.ModuleId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Module_Product_Mix_Module",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Module_Product_Mix_Product",
                        columns: x => new { x.ProductId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_order_item",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    PriceUnit = table.Column<string>(maxLength: 50, nullable: true),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 4000, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_order_item", x => new { x.ProductId, x.OrderId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Order_Item_Order",
                        columns: x => new { x.OrderId, x.Specificulture },
                        principalTable: "mix_order",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Item_Product",
                        columns: x => new { x.ProductId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_product",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_product", x => new { x.ProductId, x.CategoryId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Page_Product_Mix_Page",
                        columns: x => new { x.CategoryId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Page_Product_Mix_Product",
                        columns: x => new { x.ProductId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_product_media",
                columns: table => new
                {
                    MediaId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_product_media", x => new { x.MediaId, x.ProductId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_product_media_mix_media",
                        columns: x => new { x.MediaId, x.Specificulture },
                        principalTable: "mix_media",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_product_media_mix_product",
                        columns: x => new { x.ProductId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_product_module",
                columns: table => new
                {
                    ModuleId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_product_module", x => new { x.ModuleId, x.ProductId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Product_Module_Mix_Module1",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mix_Product_Module_Mix_Product",
                        columns: x => new { x.ProductId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_related_product",
                columns: table => new
                {
                    SourceId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    Image = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_related_product", x => new { x.SourceId, x.DestinationId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_related_product_mix_product1",
                        columns: x => new { x.DestinationId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_related_product_mix_product",
                        columns: x => new { x.SourceId, x.Specificulture },
                        principalTable: "mix_product",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_attribute_set",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    ArticleId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Fields = table.Column<string>(maxLength: 4000, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_attribute_set", x => new { x.Id, x.ModuleId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Module_Attribute_set_Mix_Module",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mix_Module_Attribute_set_Mix_Article_Module",
                        columns: x => new { x.ModuleId, x.ArticleId, x.Specificulture },
                        principalTable: "mix_article_module",
                        principalColumns: new[] { "ModuleId", "ArticleId", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Module_Attribute_set_Mix_Page_Module",
                        columns: x => new { x.ModuleId, x.CategoryId, x.Specificulture },
                        principalTable: "mix_page_module",
                        principalColumns: new[] { "ModuleId", "CategoryId", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_data",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    ArticleId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Fields = table.Column<string>(maxLength: 4000, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_data", x => new { x.Id, x.ModuleId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_Mix_Module_Data_Mix_Module",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mix_Module_Data_Mix_Page_Module",
                        columns: x => new { x.ModuleId, x.CategoryId, x.Specificulture },
                        principalTable: "mix_page_module",
                        principalColumns: new[] { "ModuleId", "CategoryId", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mix_Module_Data_Mix_Product_Module",
                        columns: x => new { x.ModuleId, x.ProductId, x.Specificulture },
                        principalTable: "mix_article_module",
                        principalColumns: new[] { "ModuleId", "ArticleId", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_attribute_value",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AttributeSetId = table.Column<Guid>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    DataType = table.Column<int>(nullable: false),
                    DefaultValue = table.Column<string>(type: "ntext", nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Title = table.Column<string>(maxLength: 250, nullable: true),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_attribute_value", x => new { x.Id, x.AttributeSetId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_module_attribute_value_mix_module_attribute_set",
                        columns: x => new { x.AttributeSetId, x.ModuleId, x.Specificulture },
                        principalTable: "mix_module_attribute_set",
                        principalColumns: new[] { "Id", "ModuleId", "Specificulture" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_SetAttributeId",
                table: "mix_article",
                column: "SetAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_Specificulture",
                table: "mix_article",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_media_ArticleId_Specificulture",
                table: "mix_article_media",
                columns: new[] { "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_media_MediaId_Specificulture",
                table: "mix_article_media",
                columns: new[] { "MediaId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_module_ArticleId_Specificulture",
                table: "mix_article_module",
                columns: new[] { "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_module_ModuleId_Specificulture",
                table: "mix_article_module",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_comment_ArticleId_Specificulture",
                table: "mix_comment",
                columns: new[] { "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_comment_OrderId_Specificulture",
                table: "mix_comment",
                columns: new[] { "OrderId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_comment_ProductId_Specificulture",
                table: "mix_comment",
                columns: new[] { "ProductId", "Specificulture" });

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
                name: "IX_mix_module_article_ArticleId_Specificulture",
                table: "mix_module_article",
                columns: new[] { "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_article_ModuleId_Specificulture",
                table: "mix_module_article",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_set_ModuleId_Specificulture",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_set_ModuleId_ArticleId_Specificulture",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_set_ModuleId_PageId_Specificulture",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_value_AttributeSetId_ModuleId_Specificulture",
                table: "mix_module_attribute_value",
                columns: new[] { "AttributeSetId", "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_ArticleId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_PageId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_ProductId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "ProductId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_product_ModuleId_Specificulture",
                table: "mix_module_product",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_product_ProductId_Specificulture",
                table: "mix_module_product",
                columns: new[] { "ProductId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_order_CustomerId",
                table: "mix_order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_order_Specificulture",
                table: "mix_order",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_order_item_Specificulture",
                table: "mix_order_item",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "AK_mix_order_item_Id_Specificulture",
                table: "mix_order_item",
                columns: new[] { "Id", "Specificulture" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mix_order_item_OrderId_Specificulture",
                table: "mix_order_item",
                columns: new[] { "OrderId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_order_item_ProductId_Specificulture",
                table: "mix_order_item",
                columns: new[] { "ProductId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_SetAttributeId",
                table: "mix_page",
                column: "SetAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_Specificulture",
                table: "mix_page",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_article_ArticleId_Specificulture",
                table: "mix_page_article",
                columns: new[] { "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_article_PageId_Specificulture",
                table: "mix_page_article",
                columns: new[] { "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_module_PageId_Specificulture",
                table: "mix_page_module",
                columns: new[] { "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_module_ModuleId_Specificulture",
                table: "mix_page_module",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_page_Id_Specificulture",
                table: "mix_page_page",
                columns: new[] { "Id", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_page_ParentId_Specificulture",
                table: "mix_page_page",
                columns: new[] { "ParentId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_position_PageId_Specificulture",
                table: "mix_page_position",
                columns: new[] { "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_product_PageId_Specificulture",
                table: "mix_page_product",
                columns: new[] { "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_product_ProductId_Specificulture",
                table: "mix_page_product",
                columns: new[] { "ProductId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_portal_page_navigation_ParentId",
                table: "mix_portal_page_navigation",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_portal_page_position_PortalPageId",
                table: "mix_portal_page_position",
                column: "PortalPageId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_portal_page_role_PageId",
                table: "mix_portal_page_role",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_product_SetAttributeId",
                table: "mix_product",
                column: "SetAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_product_Specificulture",
                table: "mix_product",
                column: "Specificulture");

            migrationBuilder.CreateIndex(
                name: "IX_mix_product_media_MediaId_Specificulture",
                table: "mix_product_media",
                columns: new[] { "MediaId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_product_media_ProductId_Specificulture",
                table: "mix_product_media",
                columns: new[] { "ProductId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_product_module_ModuleId_Specificulture",
                table: "mix_product_module",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_product_module_ProductId_Specificulture",
                table: "mix_product_module",
                columns: new[] { "ProductId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_article_DestinationId_Specificulture",
                table: "mix_related_article",
                columns: new[] { "DestinationId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_article_SourceId_Specificulture",
                table: "mix_related_article",
                columns: new[] { "SourceId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_product_DestinationId_Specificulture",
                table: "mix_related_product",
                columns: new[] { "DestinationId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_product_SourceId_Specificulture",
                table: "mix_related_product",
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
                name: "mix_article_media");

            migrationBuilder.DropTable(
                name: "mix_cms_user");

            migrationBuilder.DropTable(
                name: "mix_comment");

            migrationBuilder.DropTable(
                name: "mix_configuration");

            migrationBuilder.DropTable(
                name: "mix_copy");

            migrationBuilder.DropTable(
                name: "mix_file");

            migrationBuilder.DropTable(
                name: "mix_language");

            migrationBuilder.DropTable(
                name: "mix_module_article");

            migrationBuilder.DropTable(
                name: "mix_module_attribute_value");

            migrationBuilder.DropTable(
                name: "mix_module_data");

            migrationBuilder.DropTable(
                name: "mix_module_product");

            migrationBuilder.DropTable(
                name: "mix_order_item");

            migrationBuilder.DropTable(
                name: "mix_page_article");

            migrationBuilder.DropTable(
                name: "mix_page_page");

            migrationBuilder.DropTable(
                name: "mix_page_position");

            migrationBuilder.DropTable(
                name: "mix_page_product");

            migrationBuilder.DropTable(
                name: "mix_parameter");

            migrationBuilder.DropTable(
                name: "mix_portal_page_navigation");

            migrationBuilder.DropTable(
                name: "mix_portal_page_position");

            migrationBuilder.DropTable(
                name: "mix_portal_page_role");

            migrationBuilder.DropTable(
                name: "mix_product_media");

            migrationBuilder.DropTable(
                name: "mix_product_module");

            migrationBuilder.DropTable(
                name: "mix_related_article");

            migrationBuilder.DropTable(
                name: "mix_related_product");

            migrationBuilder.DropTable(
                name: "mix_template");

            migrationBuilder.DropTable(
                name: "mix_url_alias");

            migrationBuilder.DropTable(
                name: "mix_module_attribute_set");

            migrationBuilder.DropTable(
                name: "mix_order");

            migrationBuilder.DropTable(
                name: "mix_position");

            migrationBuilder.DropTable(
                name: "mix_portal_page");

            migrationBuilder.DropTable(
                name: "mix_media");

            migrationBuilder.DropTable(
                name: "mix_product");

            migrationBuilder.DropTable(
                name: "mix_theme");

            migrationBuilder.DropTable(
                name: "mix_article_module");

            migrationBuilder.DropTable(
                name: "mix_page_module");

            migrationBuilder.DropTable(
                name: "mix_customer");

            migrationBuilder.DropTable(
                name: "mix_article");

            migrationBuilder.DropTable(
                name: "mix_page");

            migrationBuilder.DropTable(
                name: "mix_module");

            migrationBuilder.DropTable(
                name: "mix_set_attribute");

            migrationBuilder.DropTable(
                name: "mix_culture");
        }
    }
}
