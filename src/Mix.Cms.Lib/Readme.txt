Scaffold-DbContext "Server=115.77.190.113,4444;Database=mix_cms_struture;UID=tinku;Pwd=1234qwe@;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\Cms -force
Add-Migration -Context MixCmsContext
Update-Database -Context MixCmsContext --force

remove ntext type
remove default value