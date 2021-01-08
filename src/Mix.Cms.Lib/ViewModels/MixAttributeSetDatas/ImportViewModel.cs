using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
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
        public MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonIgnore]
        public List<MixAttributeSetValues.ImportViewModel> Values { get; set; }

        [JsonProperty("fields")]
        public List<Lib.ViewModels.MixAttributeFields.UpdateViewModel> Fields { get; set; }

        ////[JsonIgnore]
        //public List<MixAttributeSetDatas.ODataMobileViewModel> RefData { get; set; } = new List<ODataMobileViewModel>();
        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("relatedData")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> RelatedData { get; set; } = new List<MixRelatedAttributeDatas.UpdateViewModel>();

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
            Fields = Fields ?? new List<Lib.ViewModels.MixAttributeFields.UpdateViewModel>();
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
                    if (val.Field.DataType != MixDataType.Reference)
                    {
                        ParseModelValue(Data[val.AttributeFieldName], val);
                    }
                }
                Values.Add(val);
            }

            return base.ParseModel(_context, _transaction);
        }

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
                case MixDataType.DateTime:
                    return new JProperty(item.AttributeFieldName, item.DateTimeValue);

                case MixDataType.Date:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case MixDataType.Time:
                    return (new JProperty(item.AttributeFieldName, item.DateTimeValue));

                case MixDataType.Double:
                    return (new JProperty(item.AttributeFieldName, item.DoubleValue));

                case MixDataType.Boolean:
                    return (new JProperty(item.AttributeFieldName, item.BooleanValue));

                case MixDataType.Integer:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue));

                case MixDataType.Reference:
                    //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                    return (new JProperty(item.AttributeFieldName, new JArray()));

                case MixDataType.Custom:
                case MixDataType.Duration:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.Html:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Upload:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                default:
                    return (new JProperty(item.AttributeFieldName, item.StringValue));
            }
        }

        private void ParseModelValue(JToken property, MixAttributeSetValues.ImportViewModel item)
        {
            switch (item.Field.DataType)
            {
                case MixDataType.DateTime:
                    item.DateTimeValue = property.Value<DateTime?>();
                    item.StringValue = property.Value<string>();
                    break;

                case MixDataType.Date:
                    item.DateTimeValue = property.Value<DateTime?>();
                    item.StringValue = property.Value<string>();
                    break;

                case MixDataType.Time:
                    item.DateTimeValue = property.Value<DateTime?>();
                    item.StringValue = property.Value<string>();
                    break;

                case MixDataType.Double:
                    item.DoubleValue = property.Value<double?>();
                    item.StringValue = property.Value<string>();
                    break;

                case MixDataType.Boolean:
                    item.BooleanValue = property.Value<bool?>();
                    item.StringValue = property.Value<string>()?.ToLower();
                    break;

                case MixDataType.Integer:
                    item.IntegerValue = property.Value<int?>();
                    item.StringValue = property.Value<string>();
                    break;

                case MixDataType.Reference:
                    item.StringValue = property.Value<string>();
                    break;

                case MixDataType.Upload:
                    string mediaData = property.Value<string>();
                    if (mediaData.IsBase64())
                    {
                        Lib.ViewModels.MixMedias.UpdateViewModel media = new Lib.ViewModels.MixMedias.UpdateViewModel()
                        {
                            Specificulture = Specificulture,
                            Status = MixContentStatus.Published,
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

                case MixDataType.Custom:
                case MixDataType.Duration:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.Html:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
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
                if (Data.GetValue(item.Field.Name) == null)
                {
                    Data.Add(ParseValue(item));
                }
            }
        }

        #endregion Expand
    }
}