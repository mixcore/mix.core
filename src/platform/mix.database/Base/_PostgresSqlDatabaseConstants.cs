namespace Mix.Database.Base
{
    public class PostgresDatabaseConstants : IDatabaseConstants
    {
        string IDatabaseConstants.DatabaseCollation => "und-x-icu";

        string IDatabaseConstants.CharSet => "utf8";

        string IDatabaseConstants.SmallLength => "(50)";

        string IDatabaseConstants.MediumLength => "(250)";

        string IDatabaseConstants.MaxLength => "(4000)";

        string IDatabaseConstants.DateTime => "timestamp without time zone";

        string IDatabaseConstants.Guid => "uuid";

        string IDatabaseConstants.Integer => "int";

        string IDatabaseConstants.String => "varchar";

        string IDatabaseConstants.NString => "varchar";

        string IDatabaseConstants.Text => "text";

        string IDatabaseConstants.GenerateUUID => "gen_random_uuid()";
        string IDatabaseConstants.Boolean => "boolean";
    }
}
