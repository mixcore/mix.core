namespace Mix.Cms.Lib.Interfaces
{
    public interface IMvcViewModel
    {
        public int Id { get; set; }
        public string Layout { get; set; }
        public string Title { get; set; }
        public string SeoTitle { get; set; }
        public string ThumbnailUrl { get; }
        public string SeoDescription { get; set; }
        public string TemplatePath { get; }
        public string DetailsUrl { get; }
        public string BodyClass { get; }
        public ViewModels.MixTemplates.ReadListItemViewModel View { get; set; }
    }
}