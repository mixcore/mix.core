using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Graphql.Lib.Interfaces
{
    public interface ITableNameLookup
    {
        bool InsertKeyName(string friendlyName);
        string GetFriendlyName(string correctName);
    }
}
