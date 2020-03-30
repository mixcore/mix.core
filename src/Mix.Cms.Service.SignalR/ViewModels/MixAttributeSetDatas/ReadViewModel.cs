using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Service.SignalR.Models;
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
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, ReadViewModel>
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

        public ReadViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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



        #region Async

        #endregion Async

        #endregion Overrides

        #region Expands
        private void ParseValueData(MixCmsContext _context, IDbContextTransaction _transaction)
        {
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