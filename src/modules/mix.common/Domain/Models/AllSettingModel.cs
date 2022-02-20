using Mix.Common.Domain.ViewModels;

namespace Mix.Common.Models
{
    public class AllSettingModel
    {
        public GlobalSettings GlobalSettings { get; set; }
        public List<MixConfigurationContentViewModel> MixConfigurations { get; set; }
        public List<MixLanguageContent> Translator { get; set; }
    }
}
