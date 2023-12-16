namespace Mix.Services.Graphql.Lib.Interfaces
{
    public interface ITableNameLookup
    {
        bool InsertKeyName(string friendlyName);
        string GetFriendlyName(string correctName);
    }
}
