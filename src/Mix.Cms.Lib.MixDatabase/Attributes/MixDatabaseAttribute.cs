using System;

namespace Mix.Cms.Lib.MixDatabase.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MixDatabaseAttribute : Attribute
    {
        public MixDatabaseAttribute(string databaseNamme)
        {
            TableName = $"{MixConstants.CONST_MIXDB_PREFIX}{databaseNamme}";   
        }
        public string TableName { get; set; }
    }
}
