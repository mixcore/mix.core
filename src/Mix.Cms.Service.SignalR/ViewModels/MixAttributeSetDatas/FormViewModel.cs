using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Extensions;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Service.SignalR.ViewModels.MixAttributeSetDatas
{
    public class FormViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, FormViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        #endregion Models

        #region Views
        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("relatedData")]
        [JsonIgnore]
        public List<Lib.ViewModels.MixRelatedAttributeDatas.UpdateViewModel> RelatedData { get; set; } = new List<Lib.ViewModels.MixRelatedAttributeDatas.UpdateViewModel>();

        [JsonIgnore]
        public List<Lib.ViewModels.MixAttributeSetValues.UpdateViewModel> Values { get; set; }

        [JsonIgnore]
        public List<Lib.ViewModels.MixAttributeFields.UpdateViewModel> Fields { get; set; }

        [JsonIgnore]
        public List<MixAttributeSetDatas.FormViewModel> RefData { get; set; } = new List<FormViewModel>();

        

        #endregion Views

        #endregion Properties

        #region Contructors

        public FormViewModel() : base()
        {
        }

        public FormViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getValues = Lib.ViewModels.MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction);
            if (getValues.IsSucceed)
            {
                Fields = Lib.ViewModels.MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId, _context, _transaction).Data;
                Values = getValues.Data.OrderBy(a => a.Priority).ToList();
                foreach (var field in Fields.OrderBy(f => f.Priority))
                {
                    var val = Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                    if (val == null)
                    {
                        val = new Lib.ViewModels.MixAttributeSetValues.UpdateViewModel(
                            new MixAttributeSetValue()
                            {
                                AttributeFieldId = field.Id,
                                AttributeFieldName = field.Name,
                            }
                            , _context, _transaction)
                        {
                            Priority = field.Priority
                        };
                        Values.Add(val);
                    }
                    val.Priority = field.Priority;
                    val.AttributeSetName = AttributeSetName;
                }

                ParseData();
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
            Values = Values ?? Lib.ViewModels.MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            Fields = Lib.ViewModels.MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId, _context, _transaction).Data;

            foreach (var field in Fields.OrderBy(f => f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                if (val == null)
                {
                    val = new Lib.ViewModels.MixAttributeSetValues.UpdateViewModel(
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
                if (Data[val.AttributeFieldName] != null)
                {
                    if (val.Field.DataType == Lib.MixEnums.MixDataType.Reference)
                    {
                        var arr = Data[val.AttributeFieldName].Value<JArray>();
                        if (arr != null)
                        {

                            foreach (JObject objData in arr)
                            {
                                string id = objData["id"]?.Value<string>();
                                // if have id => update data, else add new
                                if (!string.IsNullOrEmpty(id))
                                {
                                    //var getData = Repository.GetSingleModel(m => m.Id == id && m.Specificulture == Specificulture, _context, _transaction);
                                    //if (getData.IsSucceed)
                                    //{
                                    //    getData.Data.Data = objData;
                                    //    RefData.Add(getData.Data);
                                    //}
                                }
                                else
                                {
                                    RefData.Add(new FormViewModel()
                                    {
                                        Specificulture = Specificulture,
                                        AttributeSetId = field.ReferenceId.Value,
                                        Data = objData
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        ParseModelValue(Data[val.AttributeFieldName], val);
                    }
                }
                else
                {
                    Data.Add(ParseValue(val));
                }
            }

            // Save Edm html
            var getAttrSet = Mix.Cms.Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModel(m => m.Name == AttributeSetName, _context, _transaction);
            var getEdm = Lib.ViewModels.MixTemplates.UpdateViewModel.GetTemplateByPath(getAttrSet.Data.EdmTemplate, Specificulture);
            var edmField = Values.FirstOrDefault(f => f.AttributeFieldName == "edm");
            if (edmField != null && getEdm.IsSucceed && !string.IsNullOrEmpty(getEdm.Data.Content))
            {
                string body = getEdm.Data.Content;
                foreach (var prop in Data.Properties())
                {
                    body = body.Replace($"[[{prop.Name}]]", Data[prop.Name].Value<string>());
                }
                var edmFile = new Lib.ViewModels.FileViewModel()
                {
                    Content = body,
                    Extension = ".html",
                    FileFolder = "edms",
                    Filename = $"{getAttrSet.Data.EdmSubject}-{Id}"
                };
                if (FileRepository.Instance.SaveWebFile(edmFile))
                {
                    Data["edm"] = edmFile.WebPath;
                    edmField.StringValue = edmFile.WebPath;
                }
            }
            //End save edm
            return base.ParseModel(_context, _transaction); ;
        }

        #region Async

        public override async Task<RepositoryResponse<FormViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.SaveModelAsync(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                ParseData();
            }
            return result;
        }

        public override RepositoryResponse<FormViewModel> SaveModel(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
                RepositoryResponse<bool> saveValues = await SaveValues(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveValues, ref result);
            }
            // Save Ref Data
            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveRefData = await SaveRefDataAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveRefData, ref result);
            }

            // Save Related Data
            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveRelated = await SaveRelatedDataAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveRelated, ref result);
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
                        item.Priority = item.Field.Priority;
                        item.DataId = parent.Id;
                        item.Specificulture = parent.Specificulture;
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
                    var saveRef = await item.SaveModelAsync(true, context, transaction);
                    if (saveRef.IsSucceed)
                    {
                        RelatedData.Add(new Lib.ViewModels.MixRelatedAttributeDatas.UpdateViewModel()
                        {
                            Id = saveRef.Data.Id,
                            ParentId = Id,
                            ParentType = (int)Lib.MixEnums.MixAttributeSetDataType.Set,
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
                    item.Priority = Lib.ViewModels.MixRelatedAttributeDatas.UpdateViewModel.Repository.Count(
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

        private JProperty ParseValue(Lib.ViewModels.MixAttributeSetValues.UpdateViewModel item)
        {
            switch (item.DataType)
            {
                case Lib.MixEnums.MixDataType.DateTime:
                    return new JProperty(item.AttributeFieldName, item.DateTimeValue);

                case Lib.MixEnums.MixDataType.Date:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case Lib.MixEnums.MixDataType.Time:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case Lib.MixEnums.MixDataType.Double:
                    return (new JProperty(item.AttributeFieldName, item.DoubleValue));

                case Lib.MixEnums.MixDataType.Boolean:
                    return (new JProperty(item.AttributeFieldName, item.BooleanValue));

                case Lib.MixEnums.MixDataType.Number:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue));

                case Lib.MixEnums.MixDataType.Reference:
                    //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                    return (new JProperty(item.AttributeFieldName, new JArray()));

                case Lib.MixEnums.MixDataType.Custom:
                case Lib.MixEnums.MixDataType.Duration:
                case Lib.MixEnums.MixDataType.PhoneNumber:
                case Lib.MixEnums.MixDataType.Text:
                case Lib.MixEnums.MixDataType.Html:
                case Lib.MixEnums.MixDataType.MultilineText:
                case Lib.MixEnums.MixDataType.EmailAddress:
                case Lib.MixEnums.MixDataType.Password:
                case Lib.MixEnums.MixDataType.Url:
                case Lib.MixEnums.MixDataType.ImageUrl:
                case Lib.MixEnums.MixDataType.CreditCard:
                case Lib.MixEnums.MixDataType.PostalCode:
                case Lib.MixEnums.MixDataType.Upload:
                case Lib.MixEnums.MixDataType.Color:
                case Lib.MixEnums.MixDataType.Icon:
                case Lib.MixEnums.MixDataType.VideoYoutube:
                case Lib.MixEnums.MixDataType.TuiEditor:
                default:
                    return (new JProperty(item.AttributeFieldName, item.StringValue));
            }
        }

        private void ParseModelValue(JToken property, Lib.ViewModels.MixAttributeSetValues.UpdateViewModel item)
        {
            switch (item.Field.DataType)
            {
                case Lib.MixEnums.MixDataType.DateTime:
                    item.DateTimeValue = property.Value<DateTime?>();
                    item.StringValue = property.Value<string>();
                    break;

                case Lib.MixEnums.MixDataType.Date:
                    item.DateTimeValue = property.Value<DateTime?>();
                    item.StringValue = property.Value<string>();
                    break;

                case Lib.MixEnums.MixDataType.Time:
                    item.DateTimeValue = property.Value<DateTime?>();
                    item.StringValue = property.Value<string>();
                    break;

                case Lib.MixEnums.MixDataType.Double:
                    item.DoubleValue = property.Value<double?>();
                    item.StringValue = property.Value<string>();
                    break;

                case Lib.MixEnums.MixDataType.Boolean:
                    item.BooleanValue = property.Value<bool?>();
                    item.StringValue = property.Value<string>().ToLower();
                    break;

                case Lib.MixEnums.MixDataType.Number:
                    item.IntegerValue = property.Value<int?>();
                    item.StringValue = property.Value<string>();
                    break;

                case Lib.MixEnums.MixDataType.Reference:
                    item.StringValue = property.Value<string>();
                    break;

                case Lib.MixEnums.MixDataType.Upload:
                    string mediaData = property.Value<string>();
                    if (mediaData.IsBase64())
                    {
                        Lib.ViewModels.MixMedias.UpdateViewModel media = new Lib.ViewModels.MixMedias.UpdateViewModel()
                        {
                            Specificulture = Specificulture,
                            Status = Lib.MixEnums.MixContentStatus.Published,
                            MediaFile = new Lib.ViewModels.FileViewModel()
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
                            Data[item.AttributeFieldName] = item.StringValue;
                        }
                    }
                    else
                    {
                        item.StringValue = mediaData;
                    }
                    break;

                case Lib.MixEnums.MixDataType.Custom:
                case Lib.MixEnums.MixDataType.Duration:
                case Lib.MixEnums.MixDataType.PhoneNumber:
                case Lib.MixEnums.MixDataType.Text:
                case Lib.MixEnums.MixDataType.Html:
                case Lib.MixEnums.MixDataType.MultilineText:
                case Lib.MixEnums.MixDataType.EmailAddress:
                case Lib.MixEnums.MixDataType.Password:
                case Lib.MixEnums.MixDataType.Url:
                case Lib.MixEnums.MixDataType.ImageUrl:
                case Lib.MixEnums.MixDataType.CreditCard:
                case Lib.MixEnums.MixDataType.PostalCode:
                case Lib.MixEnums.MixDataType.Color:
                case Lib.MixEnums.MixDataType.Icon:
                case Lib.MixEnums.MixDataType.VideoYoutube:
                case Lib.MixEnums.MixDataType.TuiEditor:
                default:
                    item.StringValue = property.Value<string>();
                    break;
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
                    context.Database.CloseConnection();transaction.Dispose();context.Dispose();
                }
            }
        }

        public override void GenerateCache(MixAttributeSetData model, FormViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            ParseData();
            base.GenerateCache(model, view, _context, _transaction);
        }

        //public override List<Task> GenerateRelatedData(MixCmsContext context, IDbContextTransaction transaction)
        //{
        //    var tasks = new List<Task>();
        //    var attrDatas = context.MixAttributeSetData.Where(m => m.MixRelatedAttributeData
        //        .Any(d => d.Specificulture == Specificulture && d.Id == Id));
        //    foreach (var item in attrDatas)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            var data = new ReadViewModel(item, context, transaction);
        //            data.RemoveCache(item, context, transaction);
        //        }));
        //    }
        //    foreach (var item in Values)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            item.RemoveCache(item.Model);
        //        }));
        //    }
        //    return tasks;
        //}

        private void ParseData()
        {
            Data = new JObject();
            foreach (var item in Values.OrderBy(v => v.Priority))
            {
                item.AttributeFieldName = item.Field.Name;
                Data.Add(ParseValue(item));
            }
        }

        #endregion Expands
    }
}