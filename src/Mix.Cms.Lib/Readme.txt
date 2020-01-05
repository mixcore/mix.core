Scaffold-DbContext "Server=localhost;Database=mix_structure;UID=sa;Pwd=1234qwe@;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\Cms -force
Add-Migration -Context MixCmsContext
Update-Database -Context MixCmsContext

remove ntext type
remove default value


// Clean Init Database

  delete from test_mix.dbo.mix_page_module
  delete from test_mix.dbo.mix_page
  delete from test_mix.dbo.mix_module_data
  delete from test_mix.dbo.mix_module
  delete from test_mix.dbo.mix_attribute_set_data
  delete from test_mix.dbo.mix_attribute_field
  delete from test_mix.dbo.mix_attribute_set_data
  delete from test_mix.dbo.mix_attribute_set