using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Cms.Lib.ViewModels.MixInit
{
    public class InitCmsViewModel
    {
        #region Properties
        [JsonProperty("connectionString")]
        public string ConnectionString
        {
            get
            {
                return IsUseLocal
                    ? IsSqlite ? SqliteDbConnectionString : LocalDbConnectionString
                    : $"Server={DataBaseServer};Database={DataBaseName}" +
                    $";UID={DataBaseUser};Pwd={DataBasePassword};MultipleActiveResultSets=true;"
                    ;
            }
        }

        [JsonProperty("dataBaseServer")]
        public string DataBaseServer { get; set; }

        [JsonProperty("dataBaseName")]
        public string DataBaseName { get; set; }

        [JsonProperty("dataBaseUser")]
        public string DataBaseUser { get; set; }

        [JsonProperty("dataBasePassword")]
        public string DataBasePassword { get; set; }

        [JsonProperty("isUseLocal")]
        public bool IsUseLocal { get; set; }

        [JsonProperty("localDbConnectionString")]
        public string LocalDbConnectionString { get; set; } =
            $"Server=(localdb)\\MSSQLLocalDB;Initial Catalog=sw-cms.db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True";

        [JsonProperty("sqliteDbConnectionString")]
        public string SqliteDbConnectionString { get; set; } = $"Data Source=sw-cms.db";

        [JsonProperty("superAdminsuperAdmin")]
        public string SuperAdmin { get; set; }

        [JsonProperty("adminPassword")]
        public string AdminPassword { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("isSqlite")]
        public bool IsSqlite { get; set; }

        [JsonProperty("culture")]
        public InitCulture Culture { get; set; }
        #endregion

        public InitCmsViewModel()
        {

        }


    }
}
