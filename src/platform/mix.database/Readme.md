## 6.Add migration
* Set DatabaseService encrypt => false
* Decrypt database.json, authentication.json
* Move to mix.database folder
*  ``` dotnet ef --startup-project ../src/applications/mixcore --project ../src/platform/mix.database migrations add Init --context SqliteMixCmsContext --output-dir Migrations/Cms/Sqlite --verbose```
* ```dotnet ef database update --startup-project ../src/applications/mixcore --project ../src/platform/mix.database --context PostgresSqlAccountContext --verbose```

### Remove mixcontent folder
### Update DatabaseService -> GetConnectionstring
### Update DatabaseService.DatabaseProvider
# Sample Connection String:
- Sqlite: "Data Source=wwwroot\\mixcontent\\settings.sqlite"
```
DatabaseProvider = MixDatabaseProvider.SQLITE;
return "Data Source=MixContent\\mix-cms.sqlite";
```

- Postgres: "Host=localhost;Database=mixcore_structure;Username=postgres;Password=myPassword"
```
DatabaseProvider = MixDatabaseProvider.PostgreSQL;
return "Host=localhost;Database=mixcore_structure;Username=postgres;Password=myPassword";
```

- MySql: "Server=localhost;port=3306;Database=mixcore_structure;User=root;Password=;"
```
DatabaseProvider = MixDatabaseProvider.MySQL;
return "Server=localhost;port=3306;Database=mixcore_structure;User=root;Password=;";
```

- SqlServer: "Server=localhost;Database=mixcore_structure;UID=sa;Pwd=myPassword;MultipleActiveResultSets=true;"
```
DatabaseProvider = MixDatabaseProvider.SQLSERVER;
return "Server=localhost;Database=mixcore_structure;UID=sa;Pwd=myPassword;MultipleActiveResultSets=true;TrustServerCertificate=True;";
```

```
Scaffold-DbContext [CONNECTION_STRING] Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models\Cms -force
ex: [CONNECTION_STRING] = "Server=localhost;Database=mixcore_structure;UID=sa;Pwd=myPassword;MultipleActiveResultSets=true;"

dotnet ef dbcontext scaffold "Host=localhost;Database=mixcore_structure;Username=my_user;Password=myPassword" Npgsql.EntityFrameworkCore.PostgreSQL  -OutputDir Models\Cms\PostgreSQL -force
Scaffold-DbContext "Host=localhost;Database=mixcore_structure;Username=postgre;Password=myPassword" Npgsql.EntityFrameworkCore.PostgreSQL -OutputDir Entities\Cms\PostgreSQL -Context PostgresqlMixCmsContext -f
ex: [CONNECTION_STRING] =  "Host=my_host;Database=mixcore_structure;Username=my_user;Password=my_pw"

Scaffold-DbContext [CONNECTION_STRING] Pomelo.EntityFrameworkCore.MySql -OutputDir [OUTPUT DIRECTORY] -Context [NAME OF CONTEXT CLASS] -f
ex: [CONNECTION_STRING] = "Server=localhost;port=3306;Database=mixcore_structure;User=root;Password=;"

```