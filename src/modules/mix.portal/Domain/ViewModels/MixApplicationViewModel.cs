using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixApplicationViewModel
        : TenantDataViewModelBase<MixCmsContext, MixApplication, int, MixApplicationViewModel>
    {
        #region Properties

        public string BaseHref { get; set; }
        public string BaseRoute { get; set; }
        public string Domain { get; set; }
        public string BaseApiUrl { get; set; }
        public int? TemplateId { get; set; }
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }

        public string DetailUrl => $"/app/{BaseRoute}";
        public string PackateFilePath { get; set; }
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

        public override async Task Validate()
        {
            await base.Validate();
            if (Context.MixApplication.Any(m => m.BaseRoute == BaseRoute && m.MixTenantId == MixTenantId && m.Id != Id))
            {
                IsValid = false;
                Errors.Add(new("BaseRoute existed"));
            }
        }

        #endregion
    }
}
