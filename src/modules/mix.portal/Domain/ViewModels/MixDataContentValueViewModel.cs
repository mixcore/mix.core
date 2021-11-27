namespace Mix.Portal.Domain.ViewModels
{
    public class MixDataContentValueViewModel
        : MultilanguageContentViewModelBase<MixCmsContext, MixDataContentValue, Guid, MixDataContentValueViewModel>
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

        #region Contructors

        public MixDataContentValueViewModel()
        {

        }

        public MixDataContentValueViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataContentValueViewModel(MixDataContentValue entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<MixDataContentValue> ParseEntity(MixCacheService cacheService = null)
        {
            Priority = Column?.Priority ?? Priority;
            DataType = Column?.DataType ?? DataType;
            
            MixDatabaseColumnName = Column?.SystemName;
            MixDatabaseColumnId = Column?.Id ?? 0;
            return base.ParseEntity(cacheService);
        }

        public override async Task ExpandView(
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null)
        {
            UowInfo ??= uowInfo;
            var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            Column = await colRepo.GetSingleAsync(MixDatabaseColumnId);
            if (MixDatabaseColumnId > 0)
            {
                Column ??= await colRepo.GetSingleAsync(MixDatabaseColumnId);
                MixDatabaseName = Column.MixDatabaseName;
            }
            else // additional field for page / post / module => id = 0
            {
                Column = new ()
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
