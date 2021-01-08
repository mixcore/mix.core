using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.GraphQL.Infrastructure.Interfaces
{
    public interface IDatabaseMetadata
    {
        void ReloadMetadata();
        IEnumerable<TableMetadata> GetTableMetadatas();
    }
}
