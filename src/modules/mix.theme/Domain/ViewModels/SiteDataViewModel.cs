using Mix.Database.Entities.Cms;
using System.Collections.Generic;

namespace Mix.Theme.Domain.ViewModels
{
    public class SiteDataViewModel
    {
        public string CreatedBy { get; set; }

        public string Specificulture { get; set; }

        public string ThemeName { get; set; }

        public List<MixConfiguration> Configurations { get; set; }
        public List<MixLanguage> Languages { get; set; }
        public List<MixModule> Modules { get; set; }
        public List<MixModuleData> ModuleDatas { get; set; }
        public List<MixPage> Pages{ get; set; }
        public List<MixPost> Posts { get; set; }
        public List<MixDatabase> Databases { get; set; }
        public List<MixDatabaseColumn> DatabaseColumns { get; set; }
        public List<MixData> MixDatas { get; set; }
        //TODO : double check mixdata content

        public SiteDataViewModel()
        {
        }

        
    }
}