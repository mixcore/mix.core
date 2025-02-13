using Mix.Database.Entities.Settings;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json;

namespace Mix.Lib.ViewModels
{
    public sealed class MixGlobalSettingViewModel
        : SimpleViewModelBase<GlobalSettingContext, MixGlobalSetting, int, MixGlobalSettingViewModel>
    {
        #region Properties

        public DateTime? LastModified { get; set; }
        public string ServiceName { get; set; }
        public string SectionName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int TenantId { get; set; }
        public string SystemName { get; set; }
        public string Settings { get; set; }
        public bool IsEncrypt { get; set; }

        #endregion

        #region Constructors

        public MixGlobalSettingViewModel()
        {
        }

        public MixGlobalSettingViewModel(MixGlobalSetting entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixGlobalSettingViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        public override Task<MixGlobalSetting> ParseEntity(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(Settings))
            {
                if (Settings.IsJsonString())
                {
                    Settings = JObject.Parse(Settings).ToString(Formatting.None);
                }
            }
            return base.ParseEntity(cancellationToken);
        }
        public override Task ExpandView(CancellationToken cancellationToken = default)
        {
            return base.ExpandView(cancellationToken);
        }
        #endregion
    }
}
