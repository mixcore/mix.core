using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Lib.ViewModels
{
    public class AdditionalDataContentViewModel
        : MultilanguageSEOContentViewModelBase<MixCmsContext, MixDataContent, Guid>
    {
        #region Contructors

        public AdditionalDataContentViewModel()
        {
        }

        public AdditionalDataContentViewModel(Repository<MixCmsContext, MixDataContent, Guid> repository) : base(repository)
        {
        }

        public AdditionalDataContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public AdditionalDataContentViewModel(MixDataContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }


        #endregion

        #region Properties
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public List<MixDatabaseColumnViewModel> Columns { get; set; }
        public List<MixDataContentValueViewModel> Values { get; set; }
        public JObject Data { get; set; }

        public List<MixDataContentViewModel> ChildData { get; set; } = new();
        public List<MixDataContentAssociationViewModel> RelatedData { get; set; } = new();

        #endregion


        public override async Task<Guid> CreateParentAsync()
        {
            MixDataViewModel parent = new MixDataViewModel(UowInfo)
            {
                Id = Guid.NewGuid(),
                CreatedDateTime = DateTime.UtcNow,
                MixSiteId = 1,
                MixDatabaseId = MixDatabaseId,
                MixDatabaseName = MixDatabaseName,
                CreatedBy = CreatedBy,
                DisplayName = Title,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }
    }
}
