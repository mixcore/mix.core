using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDatas
{
    public class FormViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseData, FormViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("mixDatabaseId")]
        public int MixDatabaseId { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isClone")]
        public bool IsClone { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl
        {
            get => !string.IsNullOrEmpty(Id) && HasValue("seo_url")
                    ? $"/data/{Specificulture}/{MixDatabaseName}/{Property<string>("seo_url")}"
                    : null;
        }

        [JsonProperty("obj")]
        public JObject Obj { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixDatabaseParentType ParentType { get; set; }

        [JsonProperty("relatedData")]
        public List<MixDatabaseDataAssociations.UpdateViewModel> RelatedData { get; set; } = new List<MixDatabaseDataAssociations.UpdateViewModel>();

        [JsonIgnore]
        public List<MixDatabaseDataValues.UpdateViewModel> Values { get; set; }

        [JsonProperty("columns")]
        public List<MixDatabaseColumns.UpdateViewModel> Columns { get; set; }

        [JsonIgnore]
        public List<MixDatabaseDatas.FormViewModel> RefData { get; set; } = new List<FormViewModel>();

        #endregion Views

        #endregion Properties

        #region Contructors

        public FormViewModel() : base()
        {
        }

        public void UpdateValues(JObject values)
        {
            foreach (var item in values.Properties())
            {
                if (Obj.ContainsKey(item.Name))
                {
                    Obj[item.Name] = item.Value;
                }
            }
        }

        public FormViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Columns ??= MixDatabases.UpdateViewModel.Repository.GetSingleModel(f => f.Id == MixDatabaseId
           , _context, _transaction).Data.Columns;

            if (Obj == null)
            {
                Obj = Helper.ParseData(Id, Specificulture, _context, _transaction);
            }
            if (Columns.Any(c => c.DataType == MixDataType.Reference))
            {
                Obj.LoadAllReferenceData(Id, MixDatabaseId, Specificulture,
                    Columns
                    .Where(c => c.DataType == MixDataType.Reference)
                    .Select(c => new MixDatabaseColumn()
                    {
                        Name = c.Name,
                        ReferenceId = c.ReferenceId,
                        DataType = c.DataType
                    })
                    .ToList()
                        , _context, _transaction);
            }
        }

        public override MixDatabaseData ParseModel(
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
                Priority = Repository.Count(m => m.MixDatabaseName == MixDatabaseName && m.Specificulture == Specificulture, _context, _transaction).Data + 1;
            }

            if (string.IsNullOrEmpty(MixDatabaseName))
            {
                MixDatabaseName = _context.MixDatabase.First(m => m.Id == MixDatabaseId)?.Name;
            }
            if (MixDatabaseId == 0)
            {
                MixDatabaseId = _context.MixDatabase.First(m => m.Name == MixDatabaseName)?.Id ?? 0;
            }
            Values ??= MixDatabaseDataValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture
                , _context, _transaction)
                .Data.OrderBy(a => a.Priority).ToList();
            Columns ??= MixDatabaseColumns.UpdateViewModel.Repository.GetModelListBy(f => f.MixDatabaseId == MixDatabaseId
            , _context, _transaction).Data;
            Columns.AddRange(
                Values
                .Where(v => v.Column != null && !Columns.Any(f => f.Id == v.Column?.Id))
                .Select(v => v.Column)
                .ToList());
            Obj ??= new JObject();
            foreach (var field in Columns.OrderBy(f => f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.MixDatabaseColumnName == field.Name);
                if (val == null)
                {
                    val = new MixDatabaseDataValues.UpdateViewModel()
                    {
                        MixDatabaseColumnId = field.Id,
                        MixDatabaseColumnName = field.Name,
                        StringValue = field.DefaultValue,
                        Priority = field.Priority,
                        Column = field,
                        DataId = Id
                    };
                    val.ExpandView(_context, _transaction);
                    Values.Add(val);
                }
                else
                {
                    val.LastModified = DateTime.UtcNow;
                }
                val.Status = Status;
                val.Specificulture = Specificulture;
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
                                string id = objData["id"]?.Value<string>();
                                // if have id => update data, else add new
                                if (!string.IsNullOrEmpty(id))
                                {
                                    var getData = Repository.GetSingleModel(m => m.Id == id && m.Specificulture == Specificulture, _context, _transaction);
                                    if (getData.IsSucceed)
                                    {
                                        getData.Data.Obj = objData;
                                        RefData.Add(getData.Data);
                                    }
                                }
                                else
                                {
                                    RefData.Add(new FormViewModel()
                                    {
                                        Specificulture = Specificulture,
                                        MixDatabaseId = field.ReferenceId.Value,
                                        Status = MixContentStatus.Published,
                                        Obj = objData
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        val.ToModelValue(Obj[val.MixDatabaseColumnName], _context, _transaction);
                    }
                }
            }

            return base.ParseModel(_context, _transaction); ;
        }

        #region Async

        public override async Task<RepositoryResponse<FormViewModel>> SaveModelAsync(
            bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                _context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = await base.SaveModelAsync(isSaveSubModels, context, transaction);
                if (result.IsSucceed && !string.IsNullOrEmpty(ParentId) && ParentId != "0")
                {


                    var getNav = MixDatabaseDataAssociations.UpdateViewModel.Repository.GetFirstModel(
                        m => m.DataId == Id && m.ParentId == ParentId && m.ParentType == ParentType && m.Specificulture == Specificulture
                        , context, transaction);
                    if (!getNav.IsSucceed)
                    {
                        getNav.Data = new MixDatabaseDataAssociations.UpdateViewModel()
                        {
                            DataId = Id,
                            Specificulture = Specificulture,
                            MixDatabaseId = MixDatabaseId,
                            MixDatabaseName = MixDatabaseName,
                            ParentType = ParentType,
                            ParentId = ParentId,
                            Status = MixContentStatus.Published,
                        };
                        var saveResult = await getNav.Data.SaveModelAsync(false, context, transaction);

                        if (!saveResult.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = saveResult.Exception;
                            result.Errors.Add("Cannot save navigation");
                            result.Errors.AddRange(saveResult.Errors);
                        }
                    }
                    if (IsClone)
                    {
                        var cloneData = await CloneAsync(ParseModel(), Cultures, context, transaction);
                        var cloneNav = await getNav.Data.CloneAsync(getNav.Data.ParseModel(), Cultures, context, transaction);
                        if (!cloneNav.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = cloneNav.Exception;
                            result.Errors.Add("Cannot clone navigation");
                            result.Errors.AddRange(cloneNav.Errors);
                        }
                    }
                }
                if (result.IsSucceed)
                {
                    Model.CleanCache(context);
                    Obj = Helper.ParseData(Id, Specificulture, context, transaction);
                }

                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<FormViewModel>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        public override async Task<RepositoryResponse<bool>> CloneSubModelsAsync(MixDatabaseData parent, List<SupportedCulture> cloneCultures, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (Values.Count > 0)
            {
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        item.Cultures = Cultures;
                        var cloneValue = await item.CloneAsync(item.ParseModel(), Cultures, _context, _transaction);
                        ViewModelHelper.HandleResult(cloneValue, ref result);
                    }
                }
            }
            return result;
        }

        public override RepositoryResponse<FormViewModel> SaveModel(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = base.SaveModel(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                Obj = Helper.ParseData(Id, Specificulture, _context, _transaction);
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixDatabaseData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveValues = await SaveValues(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveValues, ref result);
            }
            // Save Ref Data
            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveRefData = await SaveRefDataAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveRefData, ref result);
            }

            //// Save Related Data
            //if (result.IsSucceed)
            //{
            //    RepositoryResponse<bool> saveRelated = await SaveRelatedDataAsync(parent, _context, _transaction);
            //    ViewModelHelper.HandleResult(saveRelated, ref result);
            //}

            return result;
        }

        public async Task<RepositoryResponse<bool>> SaveValues(MixDatabaseData parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        if (Columns.Any(f => f.Name == item.MixDatabaseColumnName))
                        {
                            item.DataId = parent.Id;
                            item.Specificulture = parent.Specificulture;
                            item.Priority = item.Column.Priority;
                            item.Status = MixContentStatus.Published;
                            var saveResult = await item.SaveModelAsync(false, context, transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                        }
                        else
                        {
                            var delResult = await item.RemoveModelAsync(false, context, transaction);
                            ViewModelHelper.HandleResult(delResult, ref result);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                result.Data = true;
                return result;
            }
            catch(Exception ex)
            {
                result.Errors.Add(ex.Message);
                result.Exception = ex;
                return result;
            }
            finally
            {
                if (isRoot)
                {
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        private async Task<RepositoryResponse<bool>> SaveRefDataAsync(MixDatabaseData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in RefData)
            {
                if (result.IsSucceed)
                {
                    item.Specificulture = Specificulture;
                    item.ParentId = parent.Id;
                    item.ParentType = MixDatabaseParentType.Set;
                    item.Status = MixContentStatus.Published;
                    var saveRef = await item.SaveModelAsync(true, context, transaction);
                    if (saveRef.IsSucceed)
                    {
                        RelatedData.Add(new MixDatabaseDataAssociations.UpdateViewModel()
                        {
                            DataId = saveRef.Data.Id,
                            ParentId = Id,
                            ParentType = MixDatabaseParentType.Set,
                            MixDatabaseId = saveRef.Data.MixDatabaseId,
                            MixDatabaseName = saveRef.Data.MixDatabaseName,
                            CreatedDateTime = DateTime.UtcNow,
                            Specificulture = Specificulture
                        });
                    }
                    ViewModelHelper.HandleResult(saveRef, ref result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        #endregion Async

        #endregion Overrides

        #region Expands

        public static async Task<RepositoryResponse<FormViewModel>> SaveObjectAsync(JObject data, string mixDatabaseName)
        {
            var vm = new FormViewModel()
            {
                Id = data["id"]?.Value<string>(),
                Specificulture = data["specificulture"]?.Value<string>(),
                MixDatabaseName = mixDatabaseName,
                Obj = data
            };
            return await vm.SaveModelAsync();
        }

        public bool HasValue(string fieldName)
        {
            return Obj != null && Obj.Value<string>(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            if (Obj != null)
            {
                return Obj.Value<T>(fieldName);
            }
            else
            {
                return default;
            }
        }

        #endregion Expands
    }
}