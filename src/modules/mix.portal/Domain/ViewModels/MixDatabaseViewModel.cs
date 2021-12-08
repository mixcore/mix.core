using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
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

        public MixDatabaseViewModel(MixDatabase entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null)
            : base(entity, cacheService, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(MixCacheService cacheService = null)
        {
            var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            Columns = await colRepo.GetListAsync(c => c.MixDatabaseId == Id, cacheService);
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabase parentEntity)
        {
            if (Columns != null)
            {
                foreach (var item in Columns)
                {
                    item.SetUowInfo(UowInfo); 
                    item.MixDatabaseId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.SystemName;
                    await item.SaveAsync();
                }
            }
        }

        #endregion
    }
}
