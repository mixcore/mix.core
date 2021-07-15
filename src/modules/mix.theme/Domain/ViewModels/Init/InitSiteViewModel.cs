using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.ViewModel;
using Mix.Theme.Domain.Dtos;
using System;
using System.Threading.Tasks;
using Mix.Heart.Extensions;
using System.Linq;
using Mix.Heart.Repository;

namespace Mix.Theme.Domain.ViewModels.Init
{
    public class InitSiteViewModel : ViewModelBase<MixCmsContext, MixSite, int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public InitCultureViewModel Culture { get; set; }

        private readonly Repository<MixCmsContext, MixCulture, int> _cultureRepository;
        public InitSiteViewModel(
            Repository<MixCmsContext, MixSite, int> repository,
            Repository<MixCmsContext, MixCulture, int> cultureRepository) : base(repository)
        {
            _cultureRepository = cultureRepository;
        }

        public void InitSiteData(InitCmsDto model)
        {
            Id = 1;
            DisplayName = model.SiteName;
            SystemName = model.SiteName.ToSEOString('_');
            Description = model.SiteName;
            Status = MixContentStatus.Published;
            CreatedDateTime = DateTime.UtcNow;

            Culture = new InitCultureViewModel(_cultureRepository)
            {
                Id = 1,
                Title = model.Culture.FullName,
                Specificulture = model.Culture.Specificulture,
                Alias = model.Culture.Alias,
                Icon = model.Culture.Icon,
                DisplayName = model.Culture.FullName,
                SystemName = model.Culture.Specificulture,
                Description = model.SiteName,
                Status = MixContentStatus.Published,
                CreatedDateTime = DateTime.UtcNow,
            };
        }

        protected override async Task<MixSite> SaveHandlerAsync()
        {
            var entity = await base.SaveHandlerAsync();
            return entity;
        }

        protected override async Task SaveEntityRelationshipAsync(MixSite parent)
        {
            Culture.MixSiteId = parent.Id;

            // Save and subscribe result for current consumer
            // Or can use this instead of _consumer to listen result in this viewmodel 
            // Then override ConsumeAsync to handle result
            await Culture.SaveAsync(UowInfo, _consumer);
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }
    }
}
