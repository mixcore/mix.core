using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ImportViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("cultures")]
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }
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
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Models

        #region Views
       
        [JsonProperty("obj")]
        public JObject Obj { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixEnums.MixAttributeSetDataType ParentType { get; set; }


        [JsonProperty("relatedData")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> RelatedData { get; set; } = new List<MixRelatedAttributeDatas.UpdateViewModel>();

        [JsonIgnore]
        public List<MixAttributeSetValues.UpdateViewModel> Values { get; set; }

        [JsonProperty("fields")]
        public List<MixAttributeFields.UpdateViewModel> Fields { get; set; }
        [JsonIgnore]
        public List<MixAttributeSetDatas.ImportViewModel> RefData { get; set; } = new List<ImportViewModel>();



        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Obj == null)
            {
                Obj = Helper.ParseData(Id, Specificulture, _context, _transaction);
            }
        }

        public override MixAttributeSetData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
                Priority = Repository.Count(m => m.AttributeSetName == AttributeSetName && m.Specificulture == Specificulture, _context, _transaction).Data + 1;
            }

            if (string.IsNullOrEmpty(AttributeSetName))
            {
                AttributeSetName = _context.MixAttributeSet.First(m => m.Id == AttributeSetId)?.Name;
            }
            if (AttributeSetId == 0)
            {
                AttributeSetId = _context.MixAttributeSet.First(m => m.Name == AttributeSetName)?.Id ?? 0;
            }
            Values ??= MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture).Data.OrderBy(a => a.Priority).ToList();
            Fields ??= MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId, _context, _transaction).Data;

            foreach (var field in Fields.OrderBy(f => f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                if (val == null)
                {
                    val = new MixAttributeSetValues.UpdateViewModel(
                        new MixAttributeSetValue()
                        {
                            AttributeFieldId = field.Id,
                            AttributeFieldName = field.Name,
                        }
                        , _context, _transaction)
                    {
                        StringValue = field.DefaultValue,
                        Priority = field.Priority,
                        Field = field
                    };
                    Values.Add(val);
                }
                else
                {
                    val.LastModified = DateTime.UtcNow;
                }
                val.Status = Status;
                val.Priority = field.Priority;
                val.AttributeSetName = AttributeSetName;
                if (Obj[val.AttributeFieldName] != null)
                {
                    if (val.Field.DataType == MixEnums.MixDataType.Reference)
                    {
                        var arr = Obj[val.AttributeFieldName].Value<JArray>();
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
                                        getData.Data.Obj = objData["obj"].Value<JObject>();
                                        RefData.Add(getData.Data);
                                    }
                                }
                                else
                                {
                                    RefData.Add(new ImportViewModel()
                                    {
                                        Specificulture = Specificulture,
                                        AttributeSetId = field.ReferenceId.Value,
                                        Obj = objData["obj"].Value<JObject>()
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        val.ToModelValue(Obj[val.AttributeFieldName]);
                    }
                }
                else
                {
                    Obj.Add(val.Model.ToJProperty());
                }
            }

           
            return base.ParseModel(_context, _transaction); ;
        }

        #region Async

        public override async Task<RepositoryResponse<ImportViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {

                var result = await base.SaveModelAsync(isSaveSubModels, context, transaction);
                if (result.IsSucceed && !string.IsNullOrEmpty(ParentId))
                {
                    var getNav = MixRelatedAttributeDatas.UpdateViewModel.Repository.CheckIsExists(
                        m => m.DataId == Id && m.ParentId == ParentId && m.ParentType == ParentType.ToString() && m.Specificulture == Specificulture
                        , context, transaction);
                    if (!getNav)
                    {
                        var nav = new MixRelatedAttributeDatas.UpdateViewModel()
                        {
                            DataId = Id,
                            Specificulture = Specificulture,
                            AttributeSetId = AttributeSetId,
                            AttributeSetName = AttributeSetName,
                            ParentType = ParentType,
                            ParentId = ParentId,
                            Status = MixEnums.MixContentStatus.Published
                        };
                        var saveResult = await nav.SaveModelAsync(false, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = saveResult.Exception;
                            result.Errors = saveResult.Errors;
                        }
                    }
                }

                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                if (result.IsSucceed)
                {
                    Obj = Helper.ParseData(Id, Specificulture, context, transaction);
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<ImportViewModel>(ex, isRoot, transaction);
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

        public override RepositoryResponse<ImportViewModel> SaveModel(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = base.SaveModel(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                Obj = Helper.ParseData(Id, Specificulture, _context, _transaction);
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
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

        private async Task<RepositoryResponse<bool>> SaveValues(MixAttributeSetData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Values)
            {
                if (result.IsSucceed)
                {
                    if (Fields.Any(f => f.Id == item.AttributeFieldId))
                    {
                        item.DataId = parent.Id;
                        item.Specificulture = parent.Specificulture;
                        item.Priority = item.Field.Priority;
                        item.Status = MixEnums.MixContentStatus.Published;
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
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveRefDataAsync(MixAttributeSetData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in RefData)
            {
                if (result.IsSucceed)
                {
                    item.Specificulture = Specificulture;
                    item.ParentId = parent.Id;
                    item.ParentType = MixEnums.MixAttributeSetDataType.Set;
                    item.Status = MixEnums.MixContentStatus.Published;
                    var saveRef = await item.SaveModelAsync(true, context, transaction);
                    if (saveRef.IsSucceed)
                    {
                        RelatedData.Add(new MixRelatedAttributeDatas.UpdateViewModel()
                        {
                            DataId = saveRef.Data.Id,
                            ParentId = Id,
                            ParentType = MixEnums.MixAttributeSetDataType.Set,
                            AttributeSetId = saveRef.Data.AttributeSetId,
                            AttributeSetName = saveRef.Data.AttributeSetName,
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

        public static async Task<RepositoryResponse<ImportViewModel>> SaveObjectAsync(JObject data, string attributeSetName)
        {
            var vm = new ImportViewModel()
            {
                Id = data["id"]?.Value<string>(),
                Specificulture = data["specificulture"]?.Value<string>(),
                AttributeSetName = attributeSetName,
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