using Mix.Heart.Entity;
using Mix.Shared.Enums;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixViewTemplate : EntityBase<int>
    {
        public string Content { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public MixTemplateFolderType FolderType { get; set; }
        public string Scripts { get; set; }
        public string SpaContent { get; set; }
        public string MobileContent { get; set; }
        public string Styles { get; set; }

        public string MixThemeName { get; set; }
        public int MixThemeId { get; set; }
        public virtual MixTheme MixTheme { get; set; }
    }
}
