using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Queue.EntityConfigurations
{
    internal class MixQueueMessageConfiguration : IEntityTypeConfiguration<MixQueueMessage>
    {
        public IDatabaseConstants Config;
        public MixQueueMessageConfiguration()
        {
            Config = new SqliteDatabaseConstants();
        }
        public void Configure(EntityTypeBuilder<MixQueueMessage> builder)
        {
            throw new NotImplementedException();
        }
    }
}
