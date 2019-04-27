Scaffold-DbContext "Server=localhost;Database=mix_cms_struture;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\Cms -force
Add-Migration -Context MixCmsContext
Update-Database -Context MixCmsContext --force

remove ntext type
remove default value
