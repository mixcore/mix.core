using Microsoft.EntityFrameworkCore.Migrations;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;

namespace Mix.Cms.Lib.Migrations.MySqlMixCms
{
    public partial class RenameDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (MixService.GetConfig<string>(MixConfigurations.CONST_MIXCORE_VERSION) != "1.0.1")
            {
                migrationBuilder.DropTable("mix_related_attribute_set");
                migrationBuilder.DropTable("mix_attribute_set_reference");

                migrationBuilder.RenameTable("mix_related_post", null, "mix_post_association");
                migrationBuilder.RenameTable("mix_attribute_set", null, "mix_database");
                migrationBuilder.RenameTable("mix_related_attribute_data", null, "mix_database_data_association");
                migrationBuilder.RenameTable("mix_attribute_set_value", null, "mix_database_data_value");
                migrationBuilder.RenameTable("mix_attribute_set_data", null, "mix_database_data");

                migrationBuilder.RenameColumn("AttributeFieldId", "mix_database_data_value", "MixDatabaseColumnId");
                migrationBuilder.RenameColumn("AttributeFieldName", "mix_database_data_value", "MixDatabaseColumnName");
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data_value", "MixDatabaseName");


                migrationBuilder.RenameColumn("AttributeSetId", "mix_database_data_association", "MixDatabaseId");
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data_association", "MixDatabaseName");

                migrationBuilder.RenameColumn("AttributeSetId", "mix_database_data", "MixDatabaseId");
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data", "MixDatabaseName");

                migrationBuilder.RenameTable("mix_attribute_field", null, "mix_database_column");
                migrationBuilder.RenameColumn("AttributeSetId", "mix_database_column", "MixDatabaseId");
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_column", "MixDatabaseName");
                MixService.SetConfig(MixConfigurations.CONST_MIXCORE_VERSION, "1.0.1");
                MixService.SaveSettings();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
