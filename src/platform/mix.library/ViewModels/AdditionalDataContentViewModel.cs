namespace Mix.Lib.ViewModels
{
    public sealed class AdditionalDataContentViewModel
        : HaveParentSEOContentViewModelBase<MixCmsContext, MixDataContent, Guid, AdditionalDataContentViewModel>
    {
        #region Constructors

        public AdditionalDataContentViewModel()
        {
        }

        public AdditionalDataContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public AdditionalDataContentViewModel(MixDataContent entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }


        #endregion

        #region Properties
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public List<MixDatabaseColumnViewModel> Columns { get; set; }
        public List<MixDataContentValueViewModel> Values { get; set; }
        public JObject Data { get; set; }

        public List<AdditionalDataContentViewModel> ChildData { get; set; } = new();
        public List<MixDataContentAssociationViewModel> RelatedData { get; set; } = new();

        public Guid? GuidParentId { get; set; }
        public int? IntParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

            Columns ??= await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName, cancellationToken);
            Values ??= await valRepo.GetListAsync(m => m.ParentId == Id, cancellationToken);

            Data ??= MixDataHelper.ParseData(Id, UowInfo);

            await Data.LoadAllReferenceDataAsync(Id, MixDatabaseName, UowInfo, cancellationToken: cancellationToken);
        }

        public override async Task<MixDataContent> ParseEntity(CancellationToken cancellationToken = default)
        {
            using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

            if (IsDefaultId(Id))
            {
                Id = Guid.NewGuid();
                CreatedDateTime = DateTime.UtcNow;
            }

            Columns ??= await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName, cancellationToken);
            Values = await valRepo.GetListAsync(m => m.ParentId == Id, cancellationToken);

            await ParseObjectToValuesAsync(cancellationToken);

            Title = Id.ToString();
            Content = Data.ToString(Newtonsoft.Json.Formatting.None);

            return await base.ParseEntity(cancellationToken);
        }

        protected override async Task<MixDataContent> SaveHandlerAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(MixDatabaseName))
            {
                MixDatabaseName = Context.MixDatabase.First(m => m.Id == MixDatabaseId)?.SystemName;
            }
            if (MixDatabaseId == 0)
            {
                MixDatabaseId = Context.MixDatabase.First(m => m.SystemName == MixDatabaseName)?.Id ?? 0;
            }

            var result = await base.SaveHandlerAsync(cancellationToken);

            var assoRepo = new Repository<MixCmsContext, MixDataContentAssociation, Guid, MixDataContentAssociationViewModel>(UowInfo);

            if (!MixHelper.IsDefaultId(GuidParentId) || !MixHelper.IsDefaultId(IntParentId))
            {
                var hasNav = await assoRepo.CheckIsExistsAsync(
                    m => m.DataContentId == Id
                    && (m.GuidParentId == GuidParentId || m.IntParentId == IntParentId)
                    && m.ParentType == ParentType
                    && m.Specificulture == Specificulture,
                    cancellationToken);

                if (!hasNav)
                {
                    var nav = new MixDataContentAssociationViewModel(UowInfo)
                    {
                        DataContentId = Id,
                        Specificulture = Specificulture,
                        MixDatabaseId = MixDatabaseId,
                        MixDatabaseName = MixDatabaseName,
                        ParentType = ParentType,
                        GuidParentId = GuidParentId,
                        IntParentId = IntParentId,
                        Status = MixContentStatus.Published
                    };
                    var saveResult = await nav.SaveAsync(cancellationToken);
                }
            }
            Data = MixDataHelper.ParseData(Id, UowInfo);
            return result;
        }

        protected override async Task SaveEntityRelationshipAsync(MixDataContent parentEntity, CancellationToken cancellationToken = default)
        {
            if (Values != null)
            {
                foreach (var item in Values)
                {
                    item.SetUowInfo(UowInfo);
                    item.ParentId = parentEntity.Id;
                    item.MixDatabaseId = parentEntity.MixDatabaseId;
                    item.MixDatabaseName = parentEntity.MixDatabaseName;
                    item.Specificulture = Specificulture;
                    item.ParentId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.MixDatabaseName;
                    await item.SaveAsync(cancellationToken);
                }
            }
        }

        public override async Task<Guid> CreateParentAsync(CancellationToken cancellationToken = default)
        {
            MixDataViewModel parent = new(UowInfo)
            {
                Id = Guid.NewGuid(),
                CreatedDateTime = DateTime.UtcNow,
                MixTenantId = MixTenantId,
                MixDatabaseId = MixDatabaseId,
                MixDatabaseName = MixDatabaseName,
                CreatedBy = CreatedBy,
                DisplayName = Title,
                Description = Excerpt
            };
            return await parent.SaveAsync(cancellationToken);
        }
        #endregion

        #region Helpers
        public T Property<T>(string propertyName)
        {
            return Data.ContainsKey(propertyName) ? Data.Value<T>(propertyName) : default(T);
        }

        private async Task ParseObjectToValuesAsync(CancellationToken cancellationToken = default)
        {
            Data ??= new JObject();
            foreach (var field in Columns.OrderBy(f => f.Priority))
            {
                var val = await GetFieldValueAsync(field, cancellationToken);

                if (Data[val.MixDatabaseColumnName] != null)
                {
                    if (val.Column.DataType == MixDataType.Reference)
                    {
                        var arr = Data[val.MixDatabaseColumnName].Value<JArray>();
                        val.IntegerValue = val.Column.ReferenceId;
                        val.StringValue = val.Column.ReferenceId.ToString();
                        if (arr != null)
                        {
                            foreach (JObject objData in arr)
                            {
                                Guid id = objData["id"]?.Value<Guid>() ?? Guid.Empty;
                                // if have id => update data, else add new
                                if (id != Guid.Empty)
                                {
                                    var data = await Repository.GetSingleAsync(m => m.Id == id, cancellationToken);
                                    data.Data = objData;
                                    ChildData.Add(data);
                                }
                                else
                                {
                                    ChildData.Add(new AdditionalDataContentViewModel()
                                    {
                                        Specificulture = Specificulture,
                                        MixDatabaseId = field.ReferenceId.Value,
                                        Data = objData["obj"].Value<JObject>()
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        val.ToModelValue(Data[val.MixDatabaseColumnName]);
                    }
                }
            }
        }

        private async Task<MixDataContentValueViewModel> GetFieldValueAsync(
            MixDatabaseColumnViewModel field,
            CancellationToken cancellationToken = default)
        {
            var val = Values.FirstOrDefault(v => v.MixDatabaseColumnId == field.Id);
            if (val == null)
            {
                val = new MixDataContentValueViewModel(UowInfo)
                {
                    MixDatabaseColumnId = field.Id,
                    MixDatabaseColumnName = field.SystemName,
                    StringValue = field.DefaultValue,
                    Priority = field.Priority,
                    Column = field,
                    MixDataContentId = Id,
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedBy = CreatedBy
                };
                await val.ExpandView(cancellationToken);
                Values.Add(val);
            }
            val.Status = Status;
            val.LastModified = DateTime.UtcNow;
            val.Priority = field.Priority;
            val.MixDatabaseName = MixDatabaseName;
            return val;
        }
        #endregion
    }
}
