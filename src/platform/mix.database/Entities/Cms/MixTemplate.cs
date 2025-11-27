

namespace Mix.Database.Entities.Cms
{
    public class MixTemplate : EntityBase<int>
    {
        public int TenantId { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public MixTemplateFolderType FolderType { get; set; }
        public string Scripts { get; set; }
        public string Styles { get; set; }

        public string MixThemeName { get; set; }
        public int MixThemeId { get; set; }
        public virtual MixTheme MixTheme { get; set; }
    }
}
