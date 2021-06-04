namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public static class SqliteDatabaseConstants
    {
        public class DatabaseConfiguration
        {
            public const string DatabaseCollation = "NOCASE";
            public const string CharSet = "utf8";
            public const string SmallLength = "(50)";
            public const string MediumLength = "(250)";
            public const string MaxLength = "(4000)";
        }

        public class DataTypes
        {
            public const string DateTime = "datetime";
            public const string Guid = "uniqueidentifier";
            public const string Integer = "int";
            public const string String = "varchar";
            public const string NString = "nvarchar";
            public const string Text = "ntext";
        }
    }
}
