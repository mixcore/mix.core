using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.RepoDb.Dtos;
using Mix.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mix.RepoDb.ViewModels
{
    public class RepoDbMixDatabaseColumnViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseColumn, int, RepoDbMixDatabaseColumnViewModel>
    {
        #region Properties
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDataType DataType { get; set; } = MixDataType.String;
        public int? ReferenceId { get; set; }

        public string DefaultValue { get; set; }
        public int MixDatabaseId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public JObject Configurations { get; set; }
        public ColumnConfigurations ColumnConfigurations { get; set; } = new();
        #endregion

        #region Constructors

        public RepoDbMixDatabaseColumnViewModel(AlterColumnDto dto )
        {
            ReflectionHelper.Map(dto, this);
        }
        public RepoDbMixDatabaseColumnViewModel()
        {
        }

        public RepoDbMixDatabaseColumnViewModel(UnitOfWorkInfo? unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public RepoDbMixDatabaseColumnViewModel(MixDatabaseColumn entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        public override Task<MixDatabaseColumn> ParseEntity(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ColumnConfigurations ??= new();
            Configurations = ReflectionHelper.ParseObject(ColumnConfigurations);

            return base.ParseEntity(cancellationToken);
        }

        public override void ParseView<TSource>(TSource sourceObject, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            base.ParseView(sourceObject, cancellationToken);
            ColumnConfigurations = Configurations != null ? Configurations.ToObject<ColumnConfigurations>()! : new();
            ColumnConfigurations.Editor ??= DataType.ToString();
        }

        #endregion
    }
}