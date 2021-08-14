namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerDatabaseConstants: IDatabaseConstants
    {
        string IDatabaseConstants.DatabaseCollation => "Vietnamese_CI_AS";

        string IDatabaseConstants.CharSet => "utf8";

        string IDatabaseConstants.SmallLength => "(50)";

        string IDatabaseConstants.MediumLength => "(250)";

        string IDatabaseConstants.MaxLength => "(4000)";

        string IDatabaseConstants.DateTime => "datetime";

        string IDatabaseConstants.Guid => "uniqueidentifier";

        string IDatabaseConstants.Integer => "int";

        string IDatabaseConstants.String => "varchar";

        string IDatabaseConstants.NString => "varchar";

        string IDatabaseConstants.Text => "ntext";

        string IDatabaseConstants.GenerateUUID => "newid()";
    }
}
