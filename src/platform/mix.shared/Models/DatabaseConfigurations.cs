using Mix.Heart.Enums;

namespace Mix.Shared.Models
{
    public class DatabaseConfigurations
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public bool ClearDbPool { get; set; }
        public MixDatabaseProvider DatabaseProvider { get; set; }
    }
}
