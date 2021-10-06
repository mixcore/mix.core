using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;

namespace Mix.Common.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public class MixLanguageContentViewModel
        : ViewModelBase<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel>
    {
        #region Properties

        public string Specificulture { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int MixCultureId { get; set; }
        public string DefaultContent { get; set; }
        public int MixLanguageId { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors
        public MixLanguageContentViewModel()
        {
        }

        public MixLanguageContentViewModel(MixLanguageContent entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        public MixLanguageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion
    }
}
