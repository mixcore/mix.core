Scaffold-DbContext "Server=localhost;Database=mix_structure;UID=sa;Pwd=1234qwe@;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\Cms -force
Add-Migration -Context MixCmsContext
Update-Database -Context MixCmsContext --force

remove ntext type
remove default value
