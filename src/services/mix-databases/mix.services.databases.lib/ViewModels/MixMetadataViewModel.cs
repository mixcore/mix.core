using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Helpers;
using Mix.Services.Databases.Lib.Entities;

namespace Mix.Services.Databases.Lib.ViewModels
{
    public class MixMetadataViewModel : ViewModelBase<MixServiceDatabaseDbContext, MixMetadata, int, MixMetadataViewModel>
    {
        #region Properties
        public string? Type { get; set; }
        public string Content { get; set; }
        public string SeoContent { get; set; }
        public int MixTenantId { get; set; }
        #endregion

        #region Constructors
        public MixMetadataViewModel()
        {
        }

        public MixMetadataViewModel(MixServiceDatabaseDbContext context) : base(context)
        {
        }

        public MixMetadataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixMetadataViewModel(MixMetadata entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        public override Task Validate(CancellationToken cancellationToken)
        {
            if (Context.MixMetadata.Any(m => m.MixTenantId == MixTenantId && m.Id != Id && m.Type == Type && m.Content == m.Content))
            {
                IsValid = false;
                Errors.Add(new($"Metadata '{Type} - {Content}' existed"));
            }
            return base.Validate(cancellationToken);
        }

        public override Task<MixMetadata> ParseEntity(CancellationToken cancellationToken = default)
        {
            SeoContent = SeoHelper.GetSEOString(Content);
            return base.ParseEntity(cancellationToken);
        }
        #endregion
    }
}
