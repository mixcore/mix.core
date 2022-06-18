namespace Mix.Portal.Domain.ViewModels
{
    public class MixPageModuleViewModel
        : ViewModelBase<MixCmsContext, MixPageModuleAssociation, int, MixPageModuleViewModel>
    {
        #region Properties
        public int LeftId { get; set; }
        public int RightId { get; set; }

        #endregion

        #region Constructors

        public MixPageModuleViewModel()
        {
        }

        public MixPageModuleViewModel(MixPageModuleAssociation entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixPageModuleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task Validate()
        {
            await base.Validate();
            if (IsValid)
            {
                IsValid = IsValid && !MixHelper.IsDefaultId(LeftId);
                if (!IsValid)
                {
                    Errors.Add(new("Parent Id cannot be null"));
                }
                IsValid = IsValid && !MixHelper.IsDefaultId(RightId);
                if (!IsValid)
                {
                    Errors.Add(new("Child Id cannot be null"));
                }
            }
        }

        #endregion

        #region Expands

        #endregion
    }
}
