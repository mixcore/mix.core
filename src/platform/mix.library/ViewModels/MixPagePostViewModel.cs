﻿
namespace Mix.Lib.ViewModels
{
    public sealed class MixPagePostAssociationViewModel
        : AssociationViewModelBase<MixCmsContext, MixPagePostAssociation, int, MixPagePostAssociationViewModel>
    {
        #region Properties
        public MixPostContentViewModel Post { get; set; }
        #endregion

        #region Constructors

        public MixPagePostAssociationViewModel()
        {
        }

        public MixPagePostAssociationViewModel(MixPagePostAssociation entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixPagePostAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await LoadPost();
        }
        #endregion

        #region Expands

        public async Task LoadPost()
        {
            Post = await MixPostContentViewModel.GetRepository(UowInfo, CacheService).GetSingleAsync(ChildId);
        }
        #endregion
    }
}
