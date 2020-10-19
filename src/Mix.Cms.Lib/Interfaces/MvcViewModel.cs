using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Cms.Lib.Interfaces
{
    public interface MvcViewModel
    {
        public int Id { get; set; }
        public string Layout { get; set; }
        public string SeoTitle { get; set; }
        public string ThumbnailUrl { get; }
        public string SeoDescription { get; set; }
        public string TemplatePath { get; }
        public string DetailsUrl { get; }
        public ViewModels.MixTemplates.ReadListItemViewModel View { get; set; }
    }
}
