using Microsoft.Extensions.Options;
using Mix.Cms.Lib.MixDatabase.Models;
using RepoDb.Interfaces;
using System.Data.SqlClient;
namespace Mix.Cms.Lib.MixDatabase.Repositories
{
    public class MixNavigationRepository : RepositoryBase<SqlConnection, MixNavigation>
    {
        public MixNavigationRepository(IOptions<AppSetting> settings, ICache cache, ITrace trace) : base(settings, cache, trace)
        { 
        }
    }
}
