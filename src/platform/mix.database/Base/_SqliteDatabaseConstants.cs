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

        string IDatabaseConstants.Date => "datetime";

        string IDatabaseConstants.Guid => "uniqueidentifier";

        string IDatabaseConstants.Integer => "integer";

        string IDatabaseConstants.Long => "BigInt";

        string IDatabaseConstants.String => "varchar";

        string IDatabaseConstants.NString => "varchar";

        string IDatabaseConstants.Text => "text";

        string IDatabaseConstants.GenerateUUID => "newid()";

        string IDatabaseConstants.Time => "time";

        string IDatabaseConstants.Now => "(DATETIME('now'))";

        string IDatabaseConstants.Boolean => "INTEGER";

        string IDatabaseConstants.BacktickOpen => "[";

        string IDatabaseConstants.BacktickClose => "]";
    }
}
