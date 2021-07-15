using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Portal.Domain.Base;
using Mix.Shared.Enums;
using System;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixDataContentValueViewModel
        : SiteContentViewModelBase<MixCmsContext, MixDataContentValue, Guid>
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

        public Guid MixDataContentId { get; set; }
        public int MixDatabaseColumnId { get; set; }

        public MixDatabaseColumnViewModel Column { get; set; }

        #endregion

        #region Contructors

        public MixDataContentValueViewModel()
        {

        }

        public MixDataContentValueViewModel(Repository<MixCmsContext, MixDataContentValue, Guid> repository) : base(repository)
        {
        }

        public MixDataContentValueViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataContentValueViewModel(MixDataContentValue entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<MixDataContentValue> ParseEntity<T>(T view)
        {
            Priority = Column?.Priority ?? Priority;
            DataType = Column?.DataType ?? DataType;

            MixDatabaseColumnName = Column?.SystemName;
            MixDatabaseColumnId = Column?.Id ?? 0;

            return base.ParseEntity(view);
        }

        public override async Task ExpandView()
        {
            var colRepo = new QueryRepository<MixCmsContext, MixDatabaseColumn, int>(UowInfo);
            Column = await colRepo.GetSingleViewAsync<MixDatabaseColumnViewModel>(MixDatabaseColumnId);
            if (MixDatabaseColumnId > 0)
            {
                Column ??= await colRepo.GetSingleViewAsync<MixDatabaseColumnViewModel>(MixDatabaseColumnId);
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
