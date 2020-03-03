using Mix.Cms.Api.GraphQL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Api.GraphQL.Infrastructure.Models
{
    public class TableNameLookup : ITableNameLookup
    {
        private IDictionary<string, string> _lookupTable = new Dictionary<string, string>(); public bool InsertKeyName(string correctName)
        {
            if (!_lookupTable.ContainsKey(correctName))
            {
                var friendlyName = CanonicalName(correctName);
                _lookupTable.Add(correctName, friendlyName);
                return true;
            }
            return false;
        }
        public string GetFriendlyName(string correctName)
        {
            if (!_lookupTable.TryGetValue(correctName, out string value))
                throw new Exception($"Could not get {correctName} out of the list.");
            return value;
        }
        private string CanonicalName(string correctName)
        {
            var index = correctName.LastIndexOf("_"); var result = correctName.Substring(
             index + 1,
             correctName.Length - index - 1); return Char.ToLowerInvariant(result[0]) + result.Substring(1);
        }
    }
}
