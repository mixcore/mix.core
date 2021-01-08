﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
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
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ExportViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, ExportViewModel>
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
        public List<MixAttributeSetValues.UpdateViewModel> Values { get; set; }

        [JsonIgnore]
        public List<MixAttributeFields.UpdateViewModel> Fields { get; set; }

        //[JsonIgnore]
        public List<MixAttributeSetDatas.UpdateViewModel> RefData { get; set; } = new List<UpdateViewModel>();

        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("relatedData")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> RelatedData { get; set; } = new List<MixRelatedAttributeDatas.UpdateViewModel>();

        #endregion Views

        #endregion Properties

        #region Contructors

        public ExportViewModel() : base()
        {
        }

        public ExportViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getValues = MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction);
            if (getValues.IsSucceed)
            {
                Values = getValues.Data.OrderBy(a => a.Priority).ToList();
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
            Values = Values ?? MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            Fields = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId, _context, _transaction).Data;
            if (string.IsNullOrEmpty(AttributeSetName))
            {
                AttributeSetName = _context.MixAttributeSet.First(m => m.Id == AttributeSetId)?.Name;
            }
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
                if (Data[val.AttributeFieldName] != null)
                {
                    if (val.Field.DataType == MixDataType.Reference)
                    {
                        var arr = Data[val.AttributeFieldName].Value<JArray>();
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
                                RefData.Add(new UpdateViewModel()
                                {
                                    Specificulture = Specificulture,
                                    AttributeSetId = field.ReferenceId.Value,
                                    Data = objData
                                });
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

            return base.ParseModel(_context, _transaction); ;
        }

        #endregion Overrides

        #region Expands

        private JProperty ParseValue(MixAttributeSetValues.UpdateViewModel item)
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

        private void ParseModelValue(JToken property, MixAttributeSetValues.UpdateViewModel item)
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
                    item.StringValue = property.Value<string>().ToLower();
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
            Data = new JObject();
            foreach (var item in Values.OrderBy(v => v.Priority))
            {
                item.AttributeFieldName = item.Field.Name;
                Data.Add(ParseValue(item));
            }
            Data.Add(new JProperty("createdDateTime", CreatedDateTime));
        }

        #endregion Expands
    }
}