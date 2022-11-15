namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public sealed class MixDataContentValueViewModel
        : MultilingualContentViewModelBase<MixCmsContext, MixDataContentValue, Guid, MixDataContentValueViewModel>
    {
        #region Properties

        public string MixDatabaseColumnName { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public MixEncryptType EncryptType { get; set; }

        public int MixDatabaseColumnId { get; set; }
        public int MixDatabaseId { get; set; }

        public MixDatabaseColumnViewModel Column { get; set; }

        #endregion

        #region Constructors

        public MixDataContentValueViewModel()
        {

        }

        public MixDataContentValueViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataContentValueViewModel(MixDataContentValue entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<MixDataContentValue> ParseEntity(CancellationToken cancellationToken = default)
        {
            Priority = Column?.Priority ?? Priority;
            DataType = Column?.DataType ?? DataType;

            MixDatabaseColumnName = Column?.SystemName;
            MixDatabaseColumnId = Column?.Id ?? 0;
            return base.ParseEntity(cancellationToken);
        }

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            Column = await colRepo.GetSingleAsync(MixDatabaseColumnId);
            if (MixDatabaseColumnId > 0)
            {
                Column ??= await colRepo.GetSingleAsync(MixDatabaseColumnId);
                MixDatabaseName = Column.MixDatabaseName;
            }
            else // additional field for page / post / module => id = 0
            {
                Column = new()
                {
                    DataType = DataType,
                    DisplayName = MixDatabaseColumnName,
                    SystemName = MixDatabaseColumnName,
                    Priority = Priority
                };
            }
        }

        #endregion
    }
}
