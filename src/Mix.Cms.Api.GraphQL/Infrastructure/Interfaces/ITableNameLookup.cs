using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.GraphQL.Infrastructure.Interfaces
{
    public interface ITableNameLookup
    {
        bool InsertKeyName(string friendlyName);
        string GetFriendlyName(string correctName);
    }

}
