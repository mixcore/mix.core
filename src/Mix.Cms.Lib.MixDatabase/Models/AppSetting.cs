namespace Mix.Cms.Lib.MixDatabase.Models
{
    public class AppSetting
    {
        public string ConnectionString { get; set; }
        public int CommandTimeout { get; set; }
        public int CacheItemExpiration { get; set; }
    }
}
