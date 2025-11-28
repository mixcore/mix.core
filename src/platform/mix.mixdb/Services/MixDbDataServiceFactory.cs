using Mix.Constant.Enums;
using Mix.Mixdb.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Mix.Heart.Enums;

namespace Mix.Mixdb.Services
{
    /// <summary>
    /// Factory implementation for creating IMixDbDataService instances
    /// </summary>
    public class MixDbDataServiceFactory : IMixDbDataServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MixDbDataServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public IMixDbDataService Create(MixDatabaseProvider provider, string connectionString)
        {
            IMixDbDataService srv = provider switch
            {
                MixDatabaseProvider.SCYLLADB => _serviceProvider.GetRequiredService<ScylladbDataService>(),
                _ => _serviceProvider.GetRequiredService<RepodbDataService>()
            };
            srv.InitConnection(provider, connectionString);
            return srv;
        }
    }
}
