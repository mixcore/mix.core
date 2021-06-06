using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Theme.Domain.Dtos;
using System;
using System.Threading.Tasks;

namespace Mix.Theme.Domain.ViewModels.Init
{
    public class InitSiteViewModel: CommandViewModelBase<MixCmsContext, MixSite, int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public InitCultureViewModel Culture { get; set; }
        public InitSiteViewModel(
            MixCmsContext dbContext,
            CommandRepository<MixCmsContext, MixSite, int> repo, 
            CommandRepository<MixCmsContext, MixCulture, int> cultureRepo, 
            InitCmsDto model) : base(dbContext, repo)
        {
            
            Culture = new InitCultureViewModel(dbContext, cultureRepo)
            {
                Id = 1,
                Specificulture = model.Culture.Specificulture,
                Alias = model.Culture.Alias,
                Icon = model.Culture.Icon,
                DisplayName = model.Culture.FullName,
                SystemName = model.Culture.Specificulture,
                Description = model.SiteName,
                CreatedDateTime = DateTime.UtcNow
            };
        }

        public override async Task<MixSite> SaveEntityAsync()
        {
            var entity  = await base.SaveEntityAsync();
            SaveEntityRelationship(entity);
            return entity;
        }

        protected override void SaveEntityRelationship(MixSite parentEntity)
        {
            Culture.SetUowInfo(_unitOfWorkInfo);
            Culture.MixSiteId = parentEntity.Id;
            Culture.SaveAsync().GetAwaiter().GetResult();
        }

    }
}
