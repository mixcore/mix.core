using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Base;
using System.ComponentModel.DataAnnotations;

namespace Mix.RepoDb.ViewModels
{
    public class MixDatabaseViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabase, int, MixDatabaseViewModel>
    {
        #region Properties
        [Required]
        public virtual string SystemName { get; set; }

        public MixDatabaseType Type { get; set; } = MixDatabaseType.Service;

        public List<MixDatabaseColumnViewModel> Columns { get; set; } = new();
        #endregion

        #region Constructors

        public MixDatabaseViewModel()
        {

        }

        public MixDatabaseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseViewModel(MixDatabase entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            Columns = await colRepo.GetListAsync(c => c.MixDatabaseId == Id, cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabase parentEntity, CancellationToken cancellationToken = default)
        {
            if (Columns != null)
            {
                foreach (var item in Columns)
                {
                    item.SetUowInfo(UowInfo);
                    item.MixDatabaseId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.SystemName;
                    await item.SaveAsync(cancellationToken);
                }
            }
        }

        #endregion
    }
}
