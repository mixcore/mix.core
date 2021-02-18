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
                string schema = null;

                migrationBuilder.DropForeignKey("FK_mix_attribute_set_data_mix_attribute_set", "mix_attribute_set_data", schema);
                migrationBuilder.DropForeignKey("FK_mix_attribute_field_mix_attribute_set", "mix_attribute_field", schema);
                migrationBuilder.DropForeignKey("FK_mix_attribute_field_mix_attribute_set1", "mix_attribute_field", schema);

                migrationBuilder.DropIndex("IX_mix_attribute_field_AttributeSetId", "mix_attribute_field", schema);
                migrationBuilder.DropIndex("IX_mix_attribute_field_ReferenceId", "mix_attribute_field", schema);
                migrationBuilder.DropIndex("IX_mix_attribute_set_data_AttributeSetId", "mix_attribute_set_data", schema);
                migrationBuilder.DropIndex("IX_mix_attribute_set_value_DataId", "mix_attribute_set_value", schema);

                migrationBuilder.DropTable("mix_related_attribute_set");
                migrationBuilder.DropTable("mix_attribute_set_reference");

                migrationBuilder.RenameTable("mix_related_post", schema, "mix_post_association");
                migrationBuilder.RenameTable("mix_attribute_set", schema, "mix_database");
                migrationBuilder.RenameTable("mix_related_attribute_data", schema, "mix_database_data_association");
                migrationBuilder.RenameTable("mix_attribute_set_value", schema, "mix_database_data_value");
                migrationBuilder.RenameTable("mix_attribute_set_data", schema, "mix_database_data");
                migrationBuilder.RenameTable("mix_attribute_field", schema, "mix_database_column");

                migrationBuilder.RenameColumn("AttributeSetId", "mix_database_column", "MixDatabaseId", schema);
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_column", "MixDatabaseName", schema);

                migrationBuilder.RenameColumn("AttributeFieldId", "mix_database_data_value", "MixDatabaseColumnId", schema);
                migrationBuilder.RenameColumn("AttributeFieldName", "mix_database_data_value", "MixDatabaseColumnName", schema);
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data_value", "MixDatabaseName", schema);


                migrationBuilder.RenameColumn("AttributeSetId", "mix_database_data_association", "MixDatabaseId", schema);
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data_association", "MixDatabaseName", schema);

                migrationBuilder.RenameColumn("AttributeSetId", "mix_database_data", "MixDatabaseId", schema);
                migrationBuilder.RenameColumn("AttributeSetName", "mix_database_data", "MixDatabaseName", schema);


                MixService.SetConfig(MixConfigurations.CONST_MIXCORE_VERSION, "1.0.1");
                MixService.SaveSettings();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MixService.SetConfig(MixConfigurations.CONST_MIXCORE_VERSION, "1.0.0");
            MixService.SaveSettings();
        }
    }
}
