using Microsoft.EntityFrameworkCore.Storage;
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
    public class NavigationViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, NavigationViewModel>
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
        public List<MixAttributeSetValues.NavigationViewModel> Values { get; set; }

        [JsonIgnore]
        public List<MixAttributeFields.ReadViewModel> Fields { get; set; }

        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("nav")]
        public Navigation Nav {
            get {
                if (AttributeSetName == MixConstants.AttributeSetName.NAVIGATION && Data != null)
                {
                    return Data.ToObject<Navigation>();
                }
                return null;
            }
        }

        #endregion Views

        #endregion Properties

        #region Contructors

        public NavigationViewModel() : base()
        {
        }

        public NavigationViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Data = new JObject
            {
                new JProperty("id", Id)
            };
            Values = MixAttributeSetValues.NavigationViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            foreach (var item in Values.OrderBy(v => v.Priority))
            {
                Data.Add(ParseValue(item));
            }
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        if (Fields.Any(f => f.Id == item.AttributeFieldId))
                        {
                            item.Priority = item.Field.Priority;
                            item.DataId = parent.Id;
                            item.Specificulture = parent.Specificulture;
                            var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                        }
                        else
                        {
                            var delResult = await item.RemoveModelAsync(false, _context, _transaction);
                            ViewModelHelper.HandleResult(delResult, ref result);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }

        #endregion Async

        #endregion Overrides

        //public override List<Task> GenerateRelatedData(MixCmsContext context, IDbContextTransaction transaction)
        //{
        //    var tasks = new List<Task>();
        //    var attrDatas = context.MixAttributeSetData.Where(m => m.MixRelatedAttributeData
        //        .Any(d => d.Specificulture == Specificulture && d.Id == Id));
        //    foreach (var item in attrDatas)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            var updModel = new NavigationViewModel(item, context, transaction);
        //            updModel.GenerateCache(item, updModel);
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

        #region Expands

        private JProperty ParseValue(MixAttributeSetValues.NavigationViewModel item)
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
                    JArray arr = new JArray();
                    foreach (var nav in item.DataNavs.OrderBy(d => d.Priority))
                    {
                        nav.Data.Data.Add(new JProperty("data", nav.Data.Data));
                        arr.Add(nav.Data.Data);
                    }
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

        private void ParseModelValue(JToken property, MixAttributeSetValues.NavigationViewModel item)
        {
            switch (item.DataType)
            {
                case MixEnums.MixDataType.DateTime:
                    item.DateTimeValue = property.Value<DateTime?>();
                    break;

                case MixEnums.MixDataType.Date:
                    item.DateTimeValue = property.Value<DateTime?>();
                    break;

                case MixEnums.MixDataType.Time:
                    item.DateTimeValue = property.Value<DateTime?>();
                    break;

                case MixEnums.MixDataType.Double:
                    item.DoubleValue = property.Value<double?>();
                    break;

                case MixEnums.MixDataType.Boolean:
                    item.BooleanValue = property.Value<bool?>();
                    break;

                case MixEnums.MixDataType.Number:
                    item.IntegerValue = property.Value<int?>();
                    break;

                case MixEnums.MixDataType.Reference:
                    //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";

                    //foreach (var nav in item.DataNavs)
                    //{
                    //    arr.Add(nav.Data.Data);
                    //}
                    //return (new JProperty(item.AttributeFieldName, url));
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
                case MixEnums.MixDataType.Upload:
                case MixEnums.MixDataType.Color:
                case MixEnums.MixDataType.Icon:
                case MixEnums.MixDataType.VideoYoutube:
                case MixEnums.MixDataType.TuiEditor:
                default:
                    item.StringValue = property.Value<string>();
                    break;
            }
            item.StringValue = property.Value<string>();
        }

        #endregion Expands
    }

    public class Navigation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("menu_items")]
        public List<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        [JsonIgnore]
        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("classes")]
        public string Classes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("menu_items")]
        public List<MenuItem> MenuItems { get; set; }

        public T Property<T>(string fieldName)
        {
            if (Data != null)
            {
                var field = Data.GetValue(fieldName);
                if (field != null)
                {
                    return field.Value<T>();
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }
    }
}