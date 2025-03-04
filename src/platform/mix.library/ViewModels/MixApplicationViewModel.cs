﻿namespace Mix.Lib.ViewModels
{
    public sealed class MixApplicationViewModel
        : TenantDataViewModelBase<MixCmsContext, MixApplication, int, MixApplicationViewModel>
    {
        #region Properties

        public string BaseHref { get; set; }
        public string DeployUrl { get; set; }
        public JObject AppSettings { get; set; } = new();
        public string Domain { get; set; }
        public string BaseApiUrl { get; set; }
        public int? TemplateId { get; set; }
        public string MixDatabaseName { get; set; }
        public int? MixDbId { get; set; }

        public string DetailUrl { get; set; }
        public string PackageFilePath { get; set; }
        #endregion

        #region Constructors

        public MixApplicationViewModel()
        {
        }

        public MixApplicationViewModel(MixApplication entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixApplicationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task Validate(CancellationToken cancellationToken)
        {
            if (Context.MixApplication.Any(m => m.DeployUrl == DeployUrl && m.TenantId == TenantId && m.Id != Id))
            {
                IsValid = false;
                Errors.Add(new("BaseRoute existed"));
            }
            await base.Validate(cancellationToken);
        }

        #endregion
    }
}
