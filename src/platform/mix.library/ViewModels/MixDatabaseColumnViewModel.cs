﻿using Mix.Shared.Models;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDatabaseColumnViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseColumn, int, MixDatabaseColumnViewModel>
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
        public ColumnConfigurations ColumnConfigurations { get; set; }
        #endregion

        #region Constructors

        public MixDatabaseColumnViewModel()
        {
        }

        public MixDatabaseColumnViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseColumnViewModel(MixDatabaseColumn entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        public override Task<MixDatabaseColumn> ParseEntity(CancellationToken cancellationToken = default)
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