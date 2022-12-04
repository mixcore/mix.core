using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Helpers;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Databases.Lib.ViewModels
{
    public class MixMixMetadataContentAsscociationViewModel : ViewModelBase<MixServiceDatabaseDbContext, MixMetadataContentAssociation, int, MixMixMetadataContentAsscociationViewModel>
    {
        #region Properties
        public MetadataParentType? ContentType { get; set; }
        public int ContentId { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Constructors
        public MixMixMetadataContentAsscociationViewModel()
        {
        }

        public MixMixMetadataContentAsscociationViewModel(MixServiceDatabaseDbContext context) : base(context)
        {
        }

        public MixMixMetadataContentAsscociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixMixMetadataContentAsscociationViewModel(MixMetadataContentAssociation entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
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
                    && m.ContentId == m.ContentId))
            {
                IsValid = false;
                Errors.Add(new($"Metadata '{MetadataId} - {ContentType} - {ContentId}' existed"));
            }
            return base.Validate(cancellationToken);
        }

        #endregion
    }
}
