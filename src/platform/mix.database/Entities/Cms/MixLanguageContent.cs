namespace Mix.Database.Entities.Cms
{
    public class MixLanguageContent : MultilingualUniqueNameContentBase<int>
    {
        public string DefaultContent { get; set; }

        public virtual MixLanguage MixLanguage { get; set; }
    }
}
