using Mix.Cms.Lib.Models.Cms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mix.Cms.Lib.ViewModels
{
    public class DashboardViewModel
    {
        [JsonProperty("totalPage")]
        public int TotalPage { get; set; }

        [JsonProperty("totalArticle")]
        public int TotalArticle { get; set; }

        [JsonProperty("totalProduct")]
        public int TotalProduct { get; set; }

        [JsonProperty("totalModule")]
        public int TotalModule { get; set; }

        [JsonProperty("totalUser")]
        public int TotalUser { get; set; }

        public DashboardViewModel()
        {
            using (MixCmsContext context = new MixCmsContext())
            {
                TotalPage = context.MixPage.Count();
                TotalArticle = context.MixArticle.Count();
                TotalProduct = context.MixProduct.Count();
            }
        }
    }
}
