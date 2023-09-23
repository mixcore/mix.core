## 6.Add migration
** Move to mix.database folder
** dotnet ef --startup-project ../../applications/Mixcore migrations add Init --context PostgresqlmixcmsContext --output-dir Migrations/Cms/PostgresqlMixCms

### Remove mixcontent folder
### Update DatabaseService -> GetConnectionstring
### Update DatabaseService.DatabaseProvider
# Sample Connection String:
- Sqlite: "Data Source=MixContent\\mix-cms.db"
```
DatabaseProvider = MixDatabaseProvider.SQLITE;
return "Data Source=MixContent\\mix-cms.db";
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