using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Shared.Helpers;

namespace Mix.Mixdb.ViewModels
{
    public class MixMetadataViewModel : ViewModelBase<MixDbDbContext, MixMetadata, int, MixMetadataViewModel>
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

        public MixMetadataViewModel(MixDbDbContext context) : base(context)
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
            if (Context.MixMetadata.Any(m => m.MixTenantId == MixTenantId && m.Id != Id && m.Type == Type && m.Content == Content))
            {
                IsValid = false;
                Errors.Add(new($"Metadata '{Type} - {Content}' existed"));
            }
            if (string.IsNullOrEmpty(Type))
            {
                IsValid = false;
                Errors.Add(new("Type is required"));
            }
            return base.Validate(cancellationToken);
        }

        public override Task<MixMetadata> ParseEntity(CancellationToken cancellationToken = default)
        {
            SeoContent ??= SeoHelper.GetSEOString(Content);
            return base.ParseEntity(cancellationToken);
        }
        #endregion
    }
}
