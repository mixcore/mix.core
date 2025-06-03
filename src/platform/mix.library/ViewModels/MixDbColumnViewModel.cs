using Mix.Shared.Models;
using Newtonsoft.Json;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDbColumnViewModel
        : ViewModelBase<MixCmsContext, MixDbColumn, int, MixDbColumnViewModel>
    {
        #region Properties
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string MixDbTableName { get; set; }
        public MixDataType DataType { get; set; } = MixDataType.String;
        public int? ReferenceId { get; set; }

        public string DefaultValue { get; set; }
        public int MixdbTableId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public JObject Configurations { get; set; }
        public ColumnConfigurations ColumnConfigurations { get; set; } = new();
        #endregion

        #region Constructors

        public MixDbColumnViewModel()
        {
        }

        public MixDbColumnViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbColumnViewModel(MixDbColumn entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        public override Task<MixDbColumn> ParseEntity(CancellationToken cancellationToken = default)
        {
            ColumnConfigurations ??= new();
            Configurations = ReflectionHelper.ParseObject(ColumnConfigurations);
            return base.ParseEntity(cancellationToken);
        }

        public override void ParseView<TSource>(TSource sourceObject, CancellationToken cancellationToken)
        {
            base.ParseView(sourceObject, cancellationToken);
            ColumnConfigurations ??= Configurations != null
                        ? Configurations.ToObject<ColumnConfigurations>()
                        : new();
            ColumnConfigurations.Editor ??= DataType.ToString();
        }

        #endregion
    }
}