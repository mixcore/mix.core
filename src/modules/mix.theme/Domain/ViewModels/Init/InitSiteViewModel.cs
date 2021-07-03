using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.ViewModel;
using Mix.Theme.Domain.Dtos;
using System;
using System.Threading.Tasks;
using Mix.Heart.Extensions;

namespace Mix.Theme.Domain.ViewModels.Init
{
    public class InitSiteViewModel : ViewModelBase<MixCmsContext, MixSite, int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public InitCultureViewModel Culture { get; set; }
        public InitSiteViewModel() : base()
        {
        }

        public InitSiteViewModel(InitCmsDto model) : base()
        {
            Id = 1;
            DisplayName = model.SiteName;
            SystemName = model.SiteName.ToSEOString('_');
            Description = model.SiteName;
            Status = MixContentStatus.Published;
            CreatedDateTime = DateTime.UtcNow;

            Culture = new InitCultureViewModel()
            {
                Id = 1,
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
            await Culture.SaveAsync(_unitOfWorkInfo, _consumer);
        }
    }
}
