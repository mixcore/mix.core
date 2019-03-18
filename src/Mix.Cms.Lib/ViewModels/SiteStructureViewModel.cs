using Mix.Cms.Lib.Models.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mix.Cms.Lib.ViewModels
{
    public class SiteStructureViewModel
    {
        public List<MixCulture> MixCulture { get; set; }
        public List<MixConfiguration> MixConfiguration { get; set; }        
        public List<MixPosition> MixPosition { get; set; }        
        public List<MixLanguage> MixLanguage { get; set; }
        public List<MixModule> MixModule { get; set; }
        public List<MixPage> MixPage { get; set; }
        public List<MixPageModule> MixPageModule { get; set; }
        public List<MixPagePage> MixPagePage { get; set; }
        public List<MixPagePosition> MixPagePosition { get; set; }
        public List<MixTemplate> MixTemplate { get; set; }
        public List<MixTheme> MixTheme { get; set; }
        public List<MixUrlAlias> MixUrlAlias { get; set; }
        public SiteStructureViewModel()
        {
            using(MixCmsContext context =  new MixCmsContext())
            {
                MixCulture = context.MixCulture.ToList();
                MixPosition = context.MixPosition.ToList();
                MixConfiguration = context.MixConfiguration.ToList();
                MixLanguage = context.MixLanguage.ToList();
                MixModule = context.MixModule.ToList();
                MixPage = context.MixPage.ToList();
                MixPageModule = context.MixPageModule.ToList();
                MixPagePage = context.MixPagePage.ToList();
                MixPagePosition = context.MixPagePosition.ToList();
                MixTemplate = context.MixTemplate.ToList();
                MixTheme = context.MixTheme.ToList();
                MixUrlAlias = context.MixUrlAlias.ToList();
            }
        }
    }
}
