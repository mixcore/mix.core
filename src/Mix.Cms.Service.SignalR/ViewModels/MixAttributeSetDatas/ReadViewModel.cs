using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Service.SignalR.Models;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Service.SignalR.ViewModels.MixAttributeSetDatas
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseData, ReadViewModel>
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
        public string Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("connection")]
        public MessengerConnection Connection { get; set; }

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

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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
                Values = getValues.Data.OrderBy(a => a.Priority).ToList();
                ParseValueData(_context, _transaction);
            }
            var getUser = ViewModels.MixMessengerUsers.DefaultViewModel.Repository.GetSingleModel(m => m.Id == CreatedBy);
            if (getUser.IsSucceed)
            {
                Connection = new MessengerConnection()
                {
                    Id = getUser.Data.Id,
                    Avatar = getUser.Data.Avatar,
                    Name = getUser.Data.Name
                };
            }
        }

        #endregion Overrides

        #region Expands

        private void ParseValueData(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            Fields = Lib.ViewModels.MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.MixDatabaseId == AttributeSetId, _context, _transaction).Data;
            foreach (var field in Fields.OrderBy(f => f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                if (val == null)
                {
                    val = new Lib.ViewModels.MixAttributeSetValues.UpdateViewModel(
                        new MixDatabaseDataValue()
                        {
                            MixDatabaseColumnId = field.Id,
                            MixDatabaseColumnName = field.Name,
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

        private JProperty ParseValue(Lib.ViewModels.MixAttributeSetValues.UpdateViewModel item)
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