namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlDatabaseConstants : IDatabaseConstants
    {
        string IDatabaseConstants.DatabaseCollation => "utf8_unicode_ci";

        string IDatabaseConstants.CharSet => "utf8";

        string IDatabaseConstants.SmallLength => "(50)";

        string IDatabaseConstants.MediumLength => "(250)";

        string IDatabaseConstants.MaxLength => "(4000)";

        string IDatabaseConstants.DateTime => "datetime";

        string IDatabaseConstants.Guid => "uuid";

        string IDatabaseConstants.Integer => "int";

        string IDatabaseConstants.String => "varchar";

        string IDatabaseConstants.NString => "varchar";

        string IDatabaseConstants.Text => "text";

        string IDatabaseConstants.GenerateUUID => "'uuid()'";
    }
}
