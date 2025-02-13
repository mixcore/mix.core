using Mix.Heart.Enums;
using Mix.Mixdb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Mixdb.Services
{
    public class MixDbDataServiceFactory
    {
        IEnumerable<IMixDbDataService> _dataServices;
        public MixDbDataServiceFactory(IEnumerable<IMixDbDataService> dataServices)
        {
            _dataServices = dataServices;
        }
        public IMixDbDataService? GetDataService(MixDatabaseProvider provider, string connectionString)
        {
            var srv = _dataServices.FirstOrDefault(m => m.DbProvider == provider);
            if (srv != null)
            {
                srv.Init(connectionString);
            }
            return srv;
        }
    }
}
