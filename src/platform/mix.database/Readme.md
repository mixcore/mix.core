# Sample Connection String:
- Sqlite: "Data Source=MixContent\\mix-cms.db"
```
_databaseProvider = MixDatabaseProvider.Sqlite;
_connectionString = "Data Source=MixContent\\mix-cms.db";
```

- Postgres: "Host=localhost;Database=mixcore_structure;Username=postgres;Password=myPassword"
```
_databaseProvider = MixDatabaseProvider.PostgreSQL;
_connectionString = "Host=localhost;Database=mixcore_structure;Username=postgres;Password=myPassword";
```

- MySql: "Server=localhost;port=3306;Database=mixcore_structure;User=root;Password=;"
```
_databaseProvider = MixDatabaseProvider.MySQL;
_connectionString = "Server=localhost;port=3306;Database=mixcore_structure;User=root;Password=;";
```

- SqlServer: "Server=localhost;Database=mixcore_structure;UID=sa;Pwd=myPassword;MultipleActiveResultSets=true;"
```
_databaseProvider = MixDatabaseProvider.SQLSERVER;
_connectionString = "Server=localhost;Database=mixcore_structure;UID=sa;Pwd=myPassword;MultipleActiveResultSets=true;";
```