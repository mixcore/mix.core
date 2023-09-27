using Mix.Shared.Helpers;

namespace Mixcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var mixContentFolder = new DirectoryInfo(MixFolders.MixContentFolder);

            // Clone Settings from shared folder
            if (!mixContentFolder.Exists)
            {
                MixFileHelper.UnZipFile(MixConstants.CONST_DEFAULT_MIX_CONTENT, MixFolders.MixContentFolder);
                MixFileHelper.DeleteFile(MixConstants.CONST_DEFAULT_MIX_CONTENT);
            }

            var builder = MixCmsHelper.CreateHostBuilder<Startup>(args);
            return builder;
        }
    }
}
