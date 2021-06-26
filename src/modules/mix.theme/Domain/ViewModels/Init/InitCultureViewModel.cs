using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Theme.Domain.Dtos;

namespace Mix.Theme.Domain.ViewModels.Init
{
    public class InitCultureViewModel : ViewModelBase<MixCmsContext, MixCulture, int>
    {
        public string Alias { get; set; }
        public string Icon { get; set; }
        public string Lcid { get; set; }
        public string Specificulture { get; set; }
        public virtual string Image { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string SystemName { get; set; }
        public virtual string Description { get; set; }

        public int MixSiteId { get; set; }

        public InitCultureViewModel(CommandRepository<MixCmsContext, MixCulture, int> repository) : base(repository)
        {
        }
    }
}
