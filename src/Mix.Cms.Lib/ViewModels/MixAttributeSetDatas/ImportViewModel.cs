using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Models.Cms;
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

        [JsonIgnore]
        public List<MixAttributeSetValues.ImportViewModel> Values { get; set; }

        [JsonProperty("fields")]
        public List<MixAttributeFields.UpdateViewModel> Fields { get; set; }

        ////[JsonIgnore]
        //public List<MixAttributeSetDatas.ODataMobileViewModel> RefData { get; set; } = new List<ODataMobileViewModel>();
        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("relatedData")]
        public List<MixRelatedAttributeDatas.ODataMobileViewModel> RelatedData { get; set; } = new List<MixRelatedAttributeDatas.ODataMobileViewModel>();

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
            Values = MixAttributeSetValues.ImportViewModel.Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            ParseData();
        }

        public override MixAttributeSetData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            Fields = Fields ?? new List<MixAttributeFields.UpdateViewModel>();
            Values = new List<MixAttributeSetValues.ImportViewModel>();
            foreach (var field in Fields)
            {
                var val = new MixAttributeSetValues.ImportViewModel()
                {
                    AttributeFieldId = field.Id,
                    AttributeFieldName = field.Name,
                    StringValue = field.DefaultValue,
                    Priority = field.Priority,
                    Field = field
                };

                val.Priority = field.Priority;
                val.DataType = field.DataType;
                val.AttributeSetName = field.AttributeSetName;
                if (Data[val.AttributeFieldName] != null)
                {
                    if (val.Field.DataType != MixEnums.MixDataType.Reference)
                    {
                        ParseModelValue(Data[val.AttributeFieldName], val);
                    }
                }
                Values.Add(val);
            }

            return base.ParseModel(_context, _transaction);
        }

        public override void GenerateCache(MixAttributeSetData model, ImportViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
        //            data.GenerateCache(item, data, context, transaction);
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

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveValues = await SaveValues(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveValues, ref result);
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

        #endregion Async

        #endregion Overrides

        #region Expand

        private JProperty ParseValue(MixAttributeSetValues.ImportViewModel item)
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

                case MixEnums.MixDataType.Number:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue));

                case MixEnums.MixDataType.Reference:
                    //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                    return (new JProperty(item.AttributeFieldName, new JArray()));

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

        private void ParseModelValue(JToken property, MixAttributeSetValues.ImportViewModel item)
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

                case MixEnums.MixDataType.Number:
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
                            Data[item.AttributeFieldName] = item.StringValue;
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

        private void ParseData()
        {
            Data = new JObject
            {
                new JProperty("id", Id),
                new JProperty("createdDateTime", CreatedDateTime),
            };
            foreach (var item in Values.OrderBy(v => v.Priority))
            {
                item.AttributeFieldName = item.Field.Name;
                Data.Add(ParseValue(item));
            }
        }

        #endregion Expand
    }
}