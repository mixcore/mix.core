using Mix.Heart.Repository;

namespace Mix.Lib.ViewModels
{
    public class AdditionalDataContentViewModel
        : HaveParentContentViewModelBase<MixCmsContext, MixDataContent, Guid, AdditionalDataContentViewModel>
    {
        #region Contructors

        public AdditionalDataContentViewModel()
        {
        }

        public AdditionalDataContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public AdditionalDataContentViewModel(MixDataContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
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

        public override async Task ExpandView(MixCacheService cacheService = null, UnitOfWorkInfo uowInfo = null)
        {
            UowInfo ??= uowInfo;
            using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

            Columns ??= await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName, cacheService, UowInfo);
            Values ??= await valRepo.GetListAsync(m => m.ParentId == Id, cacheService, UowInfo);

            Data ??= MixDataHelper.ParseData(Id, UowInfo);

            await Data.LoadAllReferenceDataAsync(Id, MixDatabaseName, UowInfo);
        }
        public override async Task<MixDataContent> ParseEntity(MixCacheService cacheService = null)
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

            Columns ??= await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName, cacheService, UowInfo);
            Values = await valRepo.GetListAsync(m => m.ParentId == Id, cacheService, UowInfo);

            await ParseObjectToValues(cacheService);

            Title = Id.ToString();
            Content = Data.ToString(Newtonsoft.Json.Formatting.None);

            return await base.ParseEntity();
        }

        protected override async Task<MixDataContent> SaveHandlerAsync()
        {
            var result = await base.SaveHandlerAsync();

            var assoRepo = new Repository<MixCmsContext, MixDataContentAssociation, Guid, MixDataContentAssociationViewModel>(UowInfo);

            if (!MixCmsHelper.IsDefaultId(GuidParentId) || !MixCmsHelper.IsDefaultId(IntParentId))
            {
                var getNav = await assoRepo.CheckIsExistsAsync(
                    m => m.DataContentId == Id
                    && (m.GuidParentId == GuidParentId || m.IntParentId == IntParentId)
                    && m.ParentType == ParentType
                    && m.Specificulture == Specificulture);
                if (!getNav)
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
                    var saveResult = await nav.SaveAsync();
                }
            }
            Data = MixDataHelper.ParseData(Id, UowInfo);
            return result;
        }


        public override async Task<Guid> CreateParentAsync()
        {
            MixDataViewModel parent = new MixDataViewModel(UowInfo)
            {
                Id = Guid.NewGuid(),
                CreatedDateTime = DateTime.UtcNow,
                MixTenantId = 1,
                MixDatabaseId = MixDatabaseId,
                MixDatabaseName = MixDatabaseName,
                CreatedBy = CreatedBy,
                DisplayName = Title,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }
        #endregion

        #region Helpers


        private async Task ParseObjectToValues(MixCacheService cacheService = null)
        {
            Data ??= new JObject();
            foreach (var field in Columns.OrderBy(f => f.Priority))
            {
                var val = await GetFieldValue(field, cacheService);

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

        private async Task<MixDataContentValueViewModel> GetFieldValue(
            MixDatabaseColumnViewModel field,
            MixCacheService cacheService = null)
        {
            var val = Values.FirstOrDefault(v => v.MixDatabaseColumnId == field.Id);
            if (val == null)
            {
                val = new MixDataContentValueViewModel()
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
                await val.ExpandView(cacheService, UowInfo);
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
