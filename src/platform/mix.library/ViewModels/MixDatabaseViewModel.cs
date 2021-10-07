using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public class MixDatabaseViewModel
        : SiteDataViewModelBase<MixCmsContext, MixDatabase, int, MixDatabaseViewModel>
    {
        #region Properties
        [Required]
        public virtual string SystemName { get; set; }

        public MixDatabaseType Type { get; set; } = MixDatabaseType.Service;

        public List<MixDatabaseColumnViewModel> Columns { get; set; } = new();
        #endregion

        #region Contructors

        public MixDatabaseViewModel()
        {

        }

        public MixDatabaseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseViewModel(MixDatabase entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(UnitOfWorkInfo uowInfo = null)
        {
            UowInfo ??= uowInfo;
            var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            Columns = await colRepo.GetListAsync(c => c.MixDatabaseId == Id);
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabase parentEntity)
        {
            if (Columns != null)
            {
                foreach (var item in Columns)
                {
                    item.MixDatabaseId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.SystemName;
                    await item.SaveAsync(UowInfo);
                }
            }
        }

        #endregion
    }
}
