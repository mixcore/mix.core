using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Databases.Lib.Entities;

namespace Mix.Services.Databases.Lib.ViewModels
{
    public class MixMetadataViewModel : ViewModelBase<MixServiceDatabaseDbContext, MixMetadata, int, MixMetadataViewModel>
    {
        #region Properties
        public string Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
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

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
        }

        protected override async Task SaveEntityRelationshipAsync(MixMetadata parentEntity, CancellationToken cancellationToken = default)
        {
        }

        #endregion
    }
}
