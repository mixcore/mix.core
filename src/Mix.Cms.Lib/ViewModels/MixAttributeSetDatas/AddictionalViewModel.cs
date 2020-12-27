using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class AddictionalViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, AddictionalViewModel>
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
        [JsonProperty("detailsUrl")]
        public string DetailsUrl
        {
            get => !string.IsNullOrEmpty(Id) && HasValue("seo_url")
                    ? $"/data/{Specificulture}/{AttributeSetName}/{Property<string>("seo_url")}"
                    : null;
        }

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
        public List<MixAttributeSetDatas.AddictionalViewModel> RefData { get; set; } = new List<AddictionalViewModel>();



        #endregion Views

        #endregion Properties

        #region Contructors

        public AddictionalViewModel() : base()
        {
        }

        public AddictionalViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            ParseData(_context, _transaction);
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
            Values = Values ?? MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            Fields = Fields ?? MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId, _context, _transaction).Data;

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
                                    RefData.Add(new AddictionalViewModel()
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
                        ParseModelValue(Obj[val.AttributeFieldName], val);
                    }
                }
                else
                {
                    Obj.Add(ParseValue(val, _context, _transaction));
                }
            }

            // Save Edm html
            var getAttrSet = Mix.Cms.Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModel(m => m.Name == AttributeSetName, _context, _transaction);
            var getEdm = Lib.ViewModels.MixTemplates.UpdateViewModel.GetTemplateByPath(getAttrSet.Data.EdmTemplate, Specificulture);
            var edmField = Values.FirstOrDefault(f => f.AttributeFieldName == "edm");
            if (edmField != null && getEdm.IsSucceed && !string.IsNullOrEmpty(getEdm.Data.Content))
            {
                string body = getEdm.Data.Content;
                foreach (var prop in Obj.Properties())
                {
                    body = body.Replace($"[[{prop.Name}]]", Obj[prop.Name].Value<string>());
                }
                var edmFile = new FileViewModel()
                {
                    Content = body,
                    Extension = ".html",
                    FileFolder = "edms",
                    Filename = $"{getAttrSet.Data.EdmSubject}-{Id}"
                };
                if (FileRepository.Instance.SaveWebFile(edmFile))
                {
                    Obj["edm"] = edmFile.WebPath;
                    edmField.StringValue = edmFile.WebPath;
                }
            }
            //End save edm
            return base.ParseModel(_context, _transaction); ;
        }

        #region Async

        public override async Task<RepositoryResponse<AddictionalViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<AddictionalViewModel>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    ParseData(context, transaction);
                    context.Dispose();
                }
            }

        }

        public override RepositoryResponse<AddictionalViewModel> SaveModel(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = base.SaveModel(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                ParseData();
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveFields = await SaveFields(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveFields, ref result);
            }
            
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

        private async Task<RepositoryResponse<bool>> SaveFields(MixAttributeSetData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var field in Fields)
            {
                if (result.IsSucceed)
                {
                    if (field.AttributeSetName == "sys_additional_field")
                    {
                        // Add field to addictional_field set
                        var saveField = await field.SaveModelAsync(false, context, transaction);
                        var val = Values.FirstOrDefault(m => m.AttributeFieldName == field.Name);
                        ViewModelHelper.HandleResult(saveField, ref result);
                        if (result.IsSucceed)
                        {
                            val.AttributeFieldId = saveField.Data.Id;
                            val.AttributeSetName = saveField.Data.AttributeSetName;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
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

        private async Task<RepositoryResponse<bool>> SaveRelatedDataAsync(MixAttributeSetData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            foreach (var item in RelatedData)
            {
                if (result.IsSucceed)
                {
                    // Current data is child data
                    if (string.IsNullOrEmpty(item.Id))
                    {
                        item.AttributeSetId = parent.AttributeSetId;
                        item.AttributeSetName = parent.AttributeSetName;
                        item.Id = parent.Id;
                    }
                    // Current data is parent data
                    else if (string.IsNullOrEmpty(item.ParentId))
                    {
                        item.ParentId = parent.Id;
                    }
                    item.Priority = MixRelatedAttributeDatas.UpdateViewModel.Repository.Count(
                                    m => m.ParentId == Id && m.Specificulture == Specificulture, context, transaction).Data + 1;
                    item.Specificulture = Specificulture;
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(true, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
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

        private JProperty ParseValue(MixAttributeSetValues.UpdateViewModel item, MixCmsContext context, IDbContextTransaction transaction)
        {
            switch (item.DataType)
            {
                case MixEnums.MixDataType.DateTime:
                    return new JProperty(item.AttributeFieldName, item.DateTimeValue);

                case MixEnums.MixDataType.Date:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case MixEnums.MixDataType.Time:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case MixEnums.MixDataType.Double:
                    return (new JProperty(item.AttributeFieldName, item.DoubleValue));

                case MixEnums.MixDataType.Boolean:
                    return (new JProperty(item.AttributeFieldName, item.BooleanValue));

                case MixEnums.MixDataType.Integer:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue));

                case MixEnums.MixDataType.Reference:

                    var arr = new JArray();
                    return (new JProperty(item.AttributeFieldName, arr));

                case MixEnums.MixDataType.Custom:
                case MixEnums.MixDataType.Duration:
                case MixEnums.MixDataType.PhoneNumber:
                case MixEnums.MixDataType.Text:
                case MixEnums.MixDataType.Html:
                case MixEnums.MixDataType.MultilineText:
                case MixEnums.MixDataType.EmailAddress:
                case MixEnums.MixDataType.Password:
                case MixEnums.MixDataType.Url:
                case MixEnums.MixDataType.ImageUrl:
                case MixEnums.MixDataType.CreditCard:
                case MixEnums.MixDataType.PostalCode:
                case MixEnums.MixDataType.Upload:
                case MixEnums.MixDataType.Color:
                case MixEnums.MixDataType.Icon:
                case MixEnums.MixDataType.VideoYoutube:
                case MixEnums.MixDataType.TuiEditor:
                default:
                    return (new JProperty(item.AttributeFieldName, item.StringValue));
            }
        }

        private void ParseModelValue(JToken property, MixAttributeSetValues.UpdateViewModel item)
        {
            if (item.Field.IsEncrypt)
            {
                var obj = property.Value<JObject>();
                item.StringValue = obj.ToString(Formatting.None);
                item.EncryptValue = obj["data"]?.ToString();
                item.EncryptKey = obj["key"]?.ToString();
            }
            else
            {
                switch (item.Field.DataType)
                {
                    case MixEnums.MixDataType.DateTime:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Date:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Time:
                        item.DateTimeValue = property.Value<DateTime?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Double:
                        item.DoubleValue = property.Value<double?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Boolean:
                        item.BooleanValue = property.Value<bool?>();
                        item.StringValue = property.Value<string>()?.ToLower();
                        break;

                    case MixEnums.MixDataType.Integer:
                        item.IntegerValue = property.Value<int?>();
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Reference:
                        item.StringValue = property.Value<string>();
                        break;

                    case MixEnums.MixDataType.Upload:
                        string mediaData = property.Value<string>();
                        if (mediaData.IsBase64())
                        {
                            MixMedias.UpdateViewModel media = new MixMedias.UpdateViewModel()
                            {
                                Specificulture = Specificulture,
                                Status = MixEnums.MixContentStatus.Published,
                                MediaFile = new FileViewModel()
                                {
                                    FileStream = mediaData,
                                    Extension = ".png",
                                    Filename = Guid.NewGuid().ToString(),
                                    FileFolder = "Attributes"
                                }
                            };
                            var saveMedia = media.SaveModel(true);
                            if (saveMedia.IsSucceed)
                            {
                                item.StringValue = saveMedia.Data.FullPath;
                                Obj[item.AttributeFieldName] = item.StringValue;
                            }
                        }
                        else
                        {
                            item.StringValue = mediaData;
                        }
                        break;

                    case MixEnums.MixDataType.Custom:
                    case MixEnums.MixDataType.Duration:
                    case MixEnums.MixDataType.PhoneNumber:
                    case MixEnums.MixDataType.Text:
                    case MixEnums.MixDataType.Html:
                    case MixEnums.MixDataType.MultilineText:
                    case MixEnums.MixDataType.EmailAddress:
                    case MixEnums.MixDataType.Password:
                    case MixEnums.MixDataType.Url:
                    case MixEnums.MixDataType.ImageUrl:
                    case MixEnums.MixDataType.CreditCard:
                    case MixEnums.MixDataType.PostalCode:
                    case MixEnums.MixDataType.Color:
                    case MixEnums.MixDataType.Icon:
                    case MixEnums.MixDataType.VideoYoutube:
                    case MixEnums.MixDataType.TuiEditor:
                    default:
                        item.StringValue = property.Value<string>();
                        break;
                }
            }

        }

        public static Task<RepositoryResponse<List<FormViewModel>>> FilterByValueAsync(string culture, string attributeSetName
            , Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryDictionary
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.Specificulture == culture;
                List<FormViewModel> result = new List<FormViewModel>();
                foreach (var q in queryDictionary)
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m =>
                    m.Specificulture == culture && m.AttributeSetName == attributeSetName
                    && m.AttributeFieldName == q.Key && m.StringValue.Contains(q.Value);
                    valPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
                }
                var query = context.MixAttributeSetValue.Where(valPredicate);
                var data = context.MixAttributeSetData.Where(m => query.Any(q => q.DataId == m.Id) && m.Specificulture == culture);
                foreach (var item in data)
                {
                    result.Add(new FormViewModel(item, context, transaction));
                }
                return Task.FromResult(new RepositoryResponse<List<FormViewModel>>()
                {
                    IsSucceed = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(UnitOfWorkHelper<MixCmsContext>.HandleException<List<FormViewModel>>(ex, isRoot, transaction));
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }


        private void ParseData(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Obj == null)
            {
                var getValues = MixAttributeSetValues.UpdateViewModel
                       .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction);

                // if create new => init from default custom data
                if (string.IsNullOrEmpty(Id))
                {
                    Fields = Fields ?? MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId, _context, _transaction).Data;
                }
                else // load custom fields
                {
                    Fields = getValues.Data.Select(v => v.Field).OrderBy(f => f.Priority).ToList();
                }
                Values = getValues.Data.OrderBy(a => a.Priority).ToList();
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
                                StringValue = field.DefaultValue
                            }
                            , _context, _transaction)
                        {
                        };
                        Values.Add(val);
                    }
                    val.Priority = field.Priority;
                    val.DataType = field.DataType;
                    val.Field = field;
                    val.AttributeSetName = AttributeSetName;
                }

                Obj = new JObject(
                new JProperty("id", Id)
            );
                foreach (var item in Values.OrderBy(v => v.Priority))
                {
                    item.AttributeFieldName = item.Field.Name;
                    Obj.Add(ParseValue(item, _context, _transaction));
                }
            }
        }
        public static async Task<RepositoryResponse<FormViewModel>> SaveObjectAsync(JObject data, string attributeSetName)
        {
            var vm = new FormViewModel()
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
            return Obj.Value<string>(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            if (Obj != null)
            {
                return Obj.Value<T>(fieldName);
            }
            else
            {
                return default(T);
            }
        }
        #endregion Expands
    }
}