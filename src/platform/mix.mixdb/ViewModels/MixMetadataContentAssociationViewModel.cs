using Mix.Constant.Enums;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Mixdb.Entities;

namespace Mix.Mixdb.ViewModels
{
    public class MixMetadataContentAsscociationViewModel : ViewModelBase<MixDbDbContext, MixMetadataContentAssociation, int, MixMetadataContentAsscociationViewModel>
    {
        #region Properties
        public MixContentType? ContentType { get; set; }
        public int ContentId { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int MixTenantId { get; set; }

        public MixMetadataViewModel Metadata { get; set; }
        #endregion

        #region Constructors
        public MixMetadataContentAsscociationViewModel()
        {
        }

        public MixMetadataContentAsscociationViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixMetadataContentAsscociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixMetadataContentAsscociationViewModel(MixMetadataContentAssociation entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        public override Task Validate(CancellationToken cancellationToken)
        {
            if (Context.MixMetadataContentAssociation.Any(
                m => m.MixTenantId == MixTenantId
                    && m.Id != Id
                    && m.MetadataId == MetadataId
                    && m.ContentType == ContentType
                    && m.ContentId == ContentId))
            {
                IsValid = false;
                Errors.Add(new($"Metadata '{MetadataId} - {ContentType} - {ContentId}' existed"));
            }
            return base.Validate(cancellationToken);
        }

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Metadata = await MixMetadataViewModel.GetRepository(UowInfo).GetSingleAsync(m => m.Id == MetadataId, cancellationToken);
        }

        #endregion
    }
}
