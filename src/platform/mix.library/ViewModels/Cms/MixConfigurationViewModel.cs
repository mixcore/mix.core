using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixConfigurationViewModel : ViewModelBase<MixCmsContext, MixConfiguration, MixConfigurationViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public string Specificulture { get; set; }

        public string Keyword { get; set; }

        public string Category { get; set; }

        public string Value { get; set; }

        public MixDataType DataType { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Models

        #region Views

        public DataValueModel Property { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public MixConfigurationViewModel() : base()
        {
        }

        public MixConfigurationViewModel(MixConfiguration model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixConfiguration ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            Value = Property.Value;
            DataType = Property.DataType;
            if (CreatedDateTime == default)
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Property = new DataValueModel() { DataType = DataType, Value = Value, Name = Keyword };
        }

        public override async Task<RepositoryResponse<MixConfigurationViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.SaveModelAsync(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed && _context == null)
            {
                MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, "LastUpdateConfiguration", DateTime.UtcNow);
                MixAppSettingService.SaveSettings();
            }
            return result;
        }

        #endregion Overrides

        #region Expand

        public static async Task<RepositoryResponse<bool>> ImportConfigurations(List<MixConfiguration> arrConfiguration, string destCulture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                foreach (var item in arrConfiguration)
                {
                    var conf = new MixConfigurationViewModel(item, context, transaction)
                    {
                        CreatedDateTime = DateTime.UtcNow,
                        Specificulture = destCulture
                    };
                    var saveResult = await conf.SaveModelAsync(false, context, transaction);
                    result.IsSucceed = result.IsSucceed && saveResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Exception = saveResult.Exception;
                        result.Errors = saveResult.Errors;
                        break;
                    }
                }
                result.Data = true;
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixConfigurationViewModel>(ex, isRoot, transaction);
                result.IsSucceed = false;
                result.Errors = error.Errors;
                result.Exception = error.Exception;
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    context.Dispose();
                }
            }
            return result;
        }

        #endregion Expand
    }
}
