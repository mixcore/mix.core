using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;
using Mix.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixDatabaseViewModel
        : SiteDataWithContentViewModelBase<MixCmsContext, MixDatabase, int>
    {
        #region Properties

        public MixDatabaseType Type { get; set; } = MixDatabaseType.Service;

        public List<MixDatabaseColumnViewModel> Columns { get; set; } = new();
        #endregion

        #region Contructors

        public MixDatabaseViewModel()
        {

        }

        public MixDatabaseViewModel(Repository<MixCmsContext, MixDatabase, int> repository) : base(repository)
        {
        }

        public MixDatabaseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseViewModel(MixDatabase entity) : base(entity)
        {
        }

        #endregion
        
        #region Overrides

        public override async Task ExpandView()
        {
            var colRepo = new QueryRepository<MixCmsContext, MixDatabaseColumn, int>(UowInfo);
            Columns = await colRepo.GetListViewAsync<MixDatabaseColumnViewModel>(c => c.MixDatabaseId == Id);
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
