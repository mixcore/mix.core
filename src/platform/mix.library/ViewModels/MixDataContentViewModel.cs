namespace Mix.Lib.ViewModels
{
    public class MixDataContentViewModel
        : HaveParentContentViewModelBase<MixCmsContext, MixDataContent, Guid, MixDataContentViewModel>
    {
        #region Contructors

        public MixDataContentViewModel()
        {
        }

        public MixDataContentViewModel(MixDataContent entity) : base(entity)
        {
        }

        public MixDataContentViewModel(UnitOfWorkInfo unitOfWorkInfo = null) : base(unitOfWorkInfo)
        {
        }

        public MixDataContentViewModel(string lang, int cultureId, string databaseName, JObject data)
        {
            Specificulture = lang;
            MixCultureId = cultureId;
            MixDatabaseName = databaseName;
            Data = data;
        }

        public MixDataContentViewModel(MixDataContent entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Properties
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public List<MixDatabaseColumnViewModel> Columns { get; set; }
        public List<MixDataContentValueViewModel> Values { get; set; }
        public JObject Data { get; set; }

        public List<MixDataContentViewModel> ChildData { get; set; } = new();
        public List<MixDataContentAssociationViewModel> RelatedData { get; set; } = new();

        public Guid? ContentGuidParentId { get; set; }
        public int? ContentIntParentId { get; set; }
        public MixDatabaseParentType ContentParentType { get; set; }


        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

            Columns ??= await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName);
            Values ??= await valRepo.GetListAsync(m => m.ParentId == Id);

            if (Data == null)
            {
                Data = MixDataHelper.ParseData(Id, UowInfo);
            }

            await Data.LoadAllReferenceDataAsync(Id, MixDatabaseName, UowInfo);
        }

        public override async Task<MixDataContent> ParseEntity()
        {
            using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

            if (IsDefaultId(Id))
            {
                Id = Guid.NewGuid();
                CreatedDateTime = DateTime.UtcNow;
            }

            if (string.IsNullOrEmpty(MixDatabaseName))
            {
                MixDatabaseName = Context.MixDatabase.First(m => m.Id == MixDatabaseId)?.SystemName;
            }
            if (MixDatabaseId == 0)
            {
                MixDatabaseId = Context.MixDatabase.First(m => m.SystemName == MixDatabaseName)?.Id ?? 0;
            }

            Columns ??= await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName);
            Values ??= await valRepo.GetListAsync(m => m.ParentId == Id);

            await ParseObjectToValues();

            Title = Id.ToString();
            Content = Data.ToString(Newtonsoft.Json.Formatting.None);

            return await base.ParseEntity();
        }

        protected override async Task SaveEntityRelationshipAsync(MixDataContent parentEntity)
        {
            if (Values != null)
            {
                foreach (var item in Values)
                {
                    item.SetUowInfo(UowInfo);
                    item.MixDataContentId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.MixDatabaseName;
                    await item.SaveAsync();
                }
            }
        }
        #endregion

        #region Helper

        private async Task ParseObjectToValues()
        {
            Data ??= new JObject();
            foreach (var field in Columns.OrderBy(f => f.Priority))
            {
                var val = await GetFieldValue(field);

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
                                    var data = await Repository.GetSingleAsync(m => m.Id == id);
                                    data.Data = objData;
                                    ChildData.Add(data);
                                }
                                else
                                {
                                    ChildData.Add(new MixDataContentViewModel()
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

        private async Task<MixDataContentValueViewModel> GetFieldValue(MixDatabaseColumnViewModel field)
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
                await val.ExpandView();
                Values.Add(val);
            }
            val.Status = Status;
            val.LastModified = DateTime.UtcNow;
            val.Priority = field.Priority;
            val.MixDatabaseName = MixDatabaseName;
            return val;
        }

        public bool HasValue(string fieldName)
        {
            return Data != null && Data.Value<string>(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            return Data.Property(fieldName).Value<T>();
        }

        public override async Task<Guid> CreateParentAsync()
        {
            MixDataViewModel parent = new (UowInfo)
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
            return await parent.SaveAsync();
        }

        #endregion
    }
}
