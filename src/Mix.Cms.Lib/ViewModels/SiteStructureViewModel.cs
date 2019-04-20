using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mix.Cms.Lib.ViewModels
{
    public class SiteStructureViewModel
    {
        public List<MixModule> MixModule { get; set; }
        public List<MixPage> MixPage { get; set; }
        public List<MixPageModule> MixPageModule { get; set; }
        public List<MixPagePage> MixPagePage { get; set; }
        public List<MixPagePosition> MixPagePosition { get; set; }
        public List<MixUrlAlias> MixUrlAlias { get; set; }
        public SiteStructureViewModel(string culture)
        {
            using(MixCmsContext context =  new MixCmsContext())
            {
                var transaction = context.Database.BeginTransaction();
                MixModule = DefaultModelRepository<MixCmsContext, MixModule>.Instance.GetModelListBy(m=>m.Specificulture == culture, context, transaction).Data;
                MixPage = DefaultModelRepository<MixCmsContext, MixPage>.Instance.GetModelListBy(m => m.Specificulture == culture, context, transaction).Data;
                MixPageModule = DefaultModelRepository<MixCmsContext, MixPageModule>.Instance.GetModelListBy(m => m.Specificulture == culture, context, transaction).Data;
                MixPagePage = DefaultModelRepository<MixCmsContext, MixPagePage>.Instance.GetModelListBy(m => m.Specificulture == culture, context, transaction).Data;
                MixPagePosition = DefaultModelRepository<MixCmsContext, MixPagePosition>.Instance.GetModelListBy(m => m.Specificulture == culture, context, transaction).Data;
                MixUrlAlias = DefaultModelRepository<MixCmsContext, MixUrlAlias>.Instance.GetModelList(context, transaction).Data;
            }
        }
    }
}
