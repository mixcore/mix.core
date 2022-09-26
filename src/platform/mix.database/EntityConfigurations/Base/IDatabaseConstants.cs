namespace Mix.Database.EntityConfigurations
{
    public interface IDatabaseConstants
    {
        public string DatabaseCollation { get; }
        public string CharSet { get; }
        public string SmallLength { get; }
        public string MediumLength { get; }
        public string MaxLength { get; }
        public string DateTime { get; }
        public string Guid { get; }
        public string Integer { get; }
        public string String { get; }
        public string NString { get; }
        public string Boolean { get; }
        public string Text { get; }
        public string GenerateUUID { get; }
    }
}
