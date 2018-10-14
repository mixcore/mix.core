using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixSystem
{
    public class SystemConfigurationViewModel
      : ViewModelBase<MixCmsContext, MixConfiguration, SystemConfigurationViewModel>
    {
        #region Properties

        #region Models

        [Required]
        [JsonProperty("keyword")]
        public string Keyword { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }
        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("createdDatetime")]
        public DateTime CreatedDatetime { get; set; }
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain", Specificulture) ?? "/"; } }

        [JsonProperty("property")]
        public DataValueViewModel Property { get; set; }

        #endregion Views
        #endregion Properties

        #region Contructors

        public SystemConfigurationViewModel()
            : base()
        {
        }
        public SystemConfigurationViewModel(MixConfiguration model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides
        public override MixConfiguration ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDatetime == default(DateTime))
            {
                CreatedDatetime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Property = new DataValueViewModel() { DataType = DataType, Value = Value, Name = Keyword };
        }

        #endregion

        #region Expands

        public static async Task<RepositoryResponse<bool>> ImportConfigurations(List<MixConfiguration> arrConfiguration, string destCulture)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var context = new MixCmsContext();
            var transaction = context.Database.BeginTransaction();

            try
            {
                foreach (var item in arrConfiguration)
                {
                    var lang = new SystemConfigurationViewModel(item, context, transaction);
                    lang.Specificulture = destCulture;
                    var saveResult = await lang.SaveModelAsync(false, context, transaction);
                    result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors = saveResult.Errors;
                        break;
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, true, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<SystemConfigurationViewModel>(ex, true, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                context?.Dispose();
            }
            return result;
        }

        #endregion   
    }
}
