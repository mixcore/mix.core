using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Shared.Enums;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixDatabaseColumnViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseColumn, int>
    {
        #region Properties
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; } = MixDataType.Text;
        public string Configurations { get; set; }
        public int? ReferenceId { get; set; }

        public int MixDatabaseId { get; set; }
        #endregion

        #region Contructors

        public MixDatabaseColumnViewModel()
        {
        }

        public MixDatabaseColumnViewModel(Repository<MixCmsContext, MixDatabaseColumn, int> repository) : base(repository)
        {
        }

        public MixDatabaseColumnViewModel(MixDatabaseColumn entity) : base(entity)
        {
        }

        public MixDatabaseColumnViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion
    }
}