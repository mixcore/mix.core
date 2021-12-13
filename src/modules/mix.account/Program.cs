namespace Mix.Account
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return MixHelper.CreateHostBuilder<Startup>(args);
        }
    }
}
