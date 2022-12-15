namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public sealed class MixUrlAliasViewModel
        : TenantDataViewModelBase<MixCmsContext, MixUrlAlias, int, MixUrlAliasViewModel>
    {
        #region Properties
        public int? SourceContentId { get; set; }

        public Guid? SourceContentGuidId { get; set; }

        public string Alias { get; set; }

        public MixUrlAliasType Type { get; set; }
        #endregion

        #region Constructors

        public MixUrlAliasViewModel()
        {

        }

        public MixUrlAliasViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixUrlAliasViewModel(MixUrlAlias entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<MixUrlAlias> ParseEntity(CancellationToken cancellationToken = default)
        {
            DisplayName ??= Alias;
            return base.ParseEntity(cancellationToken);
        }

        #endregion
    }
}
