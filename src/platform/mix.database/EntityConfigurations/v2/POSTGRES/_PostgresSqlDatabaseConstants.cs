namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class PostgresSqlDatabaseConstants
    {
        public class DatabaseConfiguration
        {
            public const string DatabaseCollation = "und-x-icu";
            public const string CharSet = "utf8";
            public const string SmallLength = "(50)";
            public const string MediumLength = "(250)";
            public const string MaxLength = "(4000)";
        }

        public class DataTypes
        {
            public const string DateTime = "timestamp without time zone";
            public const string Guid = "uuid";
            public const string Integer = "int";
            public const string String = "varchar";
            public const string NString = "varchar";
            public const string Text = "text";
        }
    }
}
