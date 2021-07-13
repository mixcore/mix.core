using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Helpers;
using Mix.Portal.Domain.Base;
using Mix.Portal.Domain.Helpers;
using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixDataContentViewModel : SiteContentSEOViewModelBase<MixCmsContext, MixDataContent, Guid>
    {
        #region Contructors

        public MixDataContentViewModel()
        {
        }

        public MixDataContentViewModel(Repository<MixCmsContext, MixDataContent, Guid> repository) : base(repository)
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
            Obj = data;
        }
        #endregion

        #region Properties
        public int MixDatabaseId { get; set; }
        public List<MixDatabaseColumnViewModel> Columns { get; set; }
        public List<MixDataContentValueViewModel> Values { get; set; }
        public JObject Obj { get; set; }
        public List<MixDataContentViewModel> RefData { get; set; } = new();

        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            using var colRepo = new QueryRepository<MixCmsContext, MixDatabaseColumn, int>(UowInfo);
            using var valRepo = new QueryRepository<MixCmsContext, MixDataContentValue, Guid>(UowInfo);

            Columns ??= await colRepo.GetListViewAsync<MixDatabaseColumnViewModel>(m => m.MixDatabaseName == MixDatabaseName);
            Values ??= await valRepo.GetListViewAsync<MixDataContentValueViewModel>(m => m.MixDataContentId == Id);

            if (Obj == null)
            {
                Obj = MixDataHelper.ParseData(Id, UowInfo);
            }

            await Obj.LoadAllReferenceDataAsync(Id, MixDatabaseName, UowInfo);
        }

        protected override async Task SaveEntityRelationshipAsync(MixDataContent parentEntity)
        {
            if (Values != null)
            {
                foreach (var item in Values)
                {
                    item.MixDataContentId = parentEntity.Id;
                    item.MixDatabaseName = parentEntity.MixDatabaseName;
                    await item.SaveAsync(UowInfo);
                }
            }
        }

        public override async Task<MixDataContent> ParseEntity<T>(T view)
        {
            using var colRepo = new QueryRepository<MixCmsContext, MixDatabaseColumn, int>(UowInfo);
            using var valRepo = new QueryRepository<MixCmsContext, MixDataContentValue, Guid>(UowInfo);

            if (string.IsNullOrEmpty(MixDatabaseName))
            {
                MixDatabaseName = Context.MixDatabase.First(m => m.Id == MixDatabaseId)?.SystemName;
            }
            if (MixDatabaseId == 0)
            {
                MixDatabaseId = Context.MixDatabase.First(m => m.SystemName == MixDatabaseName)?.Id ?? 0;
            }

            Columns ??= await colRepo.GetListViewAsync<MixDatabaseColumnViewModel>(m => m.MixDatabaseName == MixDatabaseName);
            Values = await valRepo.GetListViewAsync<MixDataContentValueViewModel>(m => m.MixDataContentId == Id);

            Obj ??= new JObject();
            foreach (var field in Columns.OrderBy(f => f.Priority))
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
                        MixDataContentId = Id
                    };
                    await val.ExpandView();
                    Values.Add(val);
                }
                else
                {
                    val.LastModified = DateTime.UtcNow;
                }
                val.Status = Status;
                val.Priority = field.Priority;
                val.MixDatabaseName = MixDatabaseName;
                if (Obj[val.MixDatabaseColumnName] != null)
                {
                    if (val.Column.DataType == MixDataType.Reference)
                    {
                        var arr = Obj[val.MixDatabaseColumnName].Value<JArray>();
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
                                    var data = await Repository.GetSingleViewAsync<MixDataContentViewModel>(m => m.Id == id);
                                    data.Obj = objData;
                                    RefData.Add(data);
                                }
                                else
                                {
                                    RefData.Add(new MixDataContentViewModel()
                                    {
                                        Specificulture = Specificulture,
                                        MixDatabaseId = field.ReferenceId.Value,
                                        Obj = objData["obj"].Value<JObject>()
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        val.ToModelValue(Obj[val.MixDatabaseColumnName]);
                    }
                }
            }

            return await base.ParseEntity(view);
        }

        #endregion

        public bool HasValue(string fieldName)
        {
            return Obj != null ? Obj.Value<string>(fieldName) != null : false;
        }

        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(Obj, fieldName);
        }

    }
}
