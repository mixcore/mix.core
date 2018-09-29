// Licensed to the Swastika I/O Foundation under one or more agreements.
// The Swastika I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Mix.Cms.Lib.Models.Cms;
using MixCore.Cms.Lib.ViewModels.System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Services
{
    public class CmsConfiguration
    {
        public CmsConfiguration()
        {
            Configuration = LoadConfiguration();
            InitParams();
        }

        public void Init(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            InitConfigurations(_context, _transaction);
        }

        public string CmsConnectionString { get; set; }
        public string AccountConnectionString { get; set; }
        public bool IsSqlite { get; set; }
        public string Language { get; set; }
        public int DefaultStatus { get; set; }
        public List<SystemConfigurationViewModel> ListConfiguration { get; set; }
        public IConfiguration Configuration { get; set; }

        private void InitParams()
        {
            CmsConnectionString = Configuration.GetConnectionString(MixConstants.CONST_DEFAULT_CONNECTION);
            AccountConnectionString = Configuration.GetConnectionString(MixConstants.CONST_DEFAULT_CONNECTION);
            Language = Configuration[MixConstants.ConfigurationKeyword.Language];
            var stt = Configuration[MixConstants.ConfigurationKeyword.DefaultStatus]?.ToString();
            DefaultStatus = stt != null ? int.Parse(stt) : 2;
            bool.TryParse(Configuration[MixConstants.ConfigurationKeyword.IsSqlite], out bool isSqlite);
            IsSqlite = isSqlite;
        }

        private void InitConfigurations(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getConfigurations = SystemConfigurationViewModel.Repository.GetModelList(_context, _transaction);
            ListConfiguration = getConfigurations.Data ?? new List<SystemConfigurationViewModel>();
        }

        public IConfiguration LoadConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory())
               .AddJsonFile(Common.Utility.Const.CONST_FILE_APPSETTING)
               .Build();

            return configuration;
        }

        public string GetLocalString(string key, string culture = null, string defaultValue = null)
        {
            var config = ListConfiguration.Find(c => c.Keyword == key && (culture == null || c.Specificulture == culture));
            return config != null ? config.Value : defaultValue;
        }

        public int GetLocalInt(string key, string culture, int defaultValue = 0)
        {
            var config = ListConfiguration.Find(c => c.Keyword == key && c.Specificulture == culture);
            if (!int.TryParse(config?.Value, out int result))
            {
                result = defaultValue;
            }
            return result;
        }
    }
}
