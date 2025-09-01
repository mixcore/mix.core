namespace Mix.Database.Base
{
    public class SqliteDatabaseConstants : IDatabaseConstants
    {
        string IDatabaseConstants.DatabaseCollation => "NOCASE";

        string IDatabaseConstants.CharSet => "utf8";

        string IDatabaseConstants.SmallLength => "(50)";

        string IDatabaseConstants.MediumLength => "(250)";

        string IDatabaseConstants.MaxLength => "(4000)";

        string IDatabaseConstants.DateTime => "datetime";

        string IDatabaseConstants.Date => "date";

        string IDatabaseConstants.Guid => "text";

        string IDatabaseConstants.Integer => "integer";

        string IDatabaseConstants.Decimal => "decimal(18,2)";

        string IDatabaseConstants.Long => "bigint";

        string IDatabaseConstants.String => "varchar";

        string IDatabaseConstants.NString => "varchar";

        string IDatabaseConstants.Text => "text";

        string IDatabaseConstants.GenerateUUID => "hex(randomblob(16))";

        string IDatabaseConstants.Time => "time";

        string IDatabaseConstants.Now => "datetime('now')";

        string IDatabaseConstants.Boolean => "integer";

        string IDatabaseConstants.BacktickOpen => "[";

        string IDatabaseConstants.BacktickClose => "]";
    }
}
