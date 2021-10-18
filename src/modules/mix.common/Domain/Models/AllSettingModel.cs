using Mix.Common.Domain.ViewModels;
using Mix.Database.Entities.Cms;
using System.Collections.Generic;

namespace Mix.Common.Models
{
    public class AllSettingModel
    {
        public GlobalSettings GlobalSettings { get; set; }
        public List<MixConfigurationContentViewModel> MixConfigurations { get; set; }
        public List<MixLanguageContent> Translator{ get; set; }
    }
}
