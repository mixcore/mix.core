using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, ReadMvcViewModel>
    {
        #region Properties
        #region Models

        public string Id { get; set; }
        public int AttributeSetId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }

        #endregion Models
        #region Views

        //public List<MixAttributeSetValues.ReadMvcViewModel> Values { get; set; }
        public JObject ObjValue { get; set; }
        #endregion
        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            ObjValue = new JObject();
            var values = MixAttributeSetValues.ReadMvcViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            foreach (var item in values.OrderBy(v=>v.Priority))
            {
                ObjValue.Add(ParseValue(item));
            }
        }

        #endregion

        #region Expands
        JProperty ParseValue(MixAttributeSetValues.ReadMvcViewModel item)
        {
            switch (item.DataType)
            {
                case MixEnums.MixDataType.DateTime:
                    return new JProperty(item.AttributeName, item.DateTimeValue);
                case MixEnums.MixDataType.Date:
                    return (new JProperty(item.AttributeName, item.DateTimeValue));
                case MixEnums.MixDataType.Time:
                    return (new JProperty(item.AttributeName, item.DateTimeValue));
                case MixEnums.MixDataType.Currency:
                    return (new JProperty(item.AttributeName, item.DoubleValue));
                case MixEnums.MixDataType.Boolean:
                    return (new JProperty(item.AttributeName, item.BooleanValue));
                case MixEnums.MixDataType.Number:
                    return (new JProperty(item.AttributeName, item.IntegerValue));
                case MixEnums.MixDataType.Reference:
                    JArray arr = new JArray();
                    foreach (var nav in item.DataNavs)
                    {
                        arr.Add(nav.Data.ObjValue);
                    }
                    return (new JProperty(item.AttributeName, arr));
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
                    return (new JProperty(item.AttributeName, item.StringValue));
            }
        }
        #endregion
    }
}
