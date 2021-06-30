using Mix.Database.Entities.Cms.v2;
using Mix.Heart.ViewModel;
using System.Threading.Tasks;

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

        protected override async Task<MixSite> SaveHandlerAsync()
        {
            var entity = await base.SaveHandlerAsync();
            await SaveEntityRelationshipAsync(entity);
            return entity;
        }

        protected override async Task SaveEntityRelationshipAsync(MixSite parent)
        {
            Culture.MixSiteId = parent.Id;
            await Culture.SaveAsync(_unitOfWorkInfo);
        }

    }
}
