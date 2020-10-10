using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public List<MixAttributeSetValues.NavigationViewModel> Values { get; set; }

        public List<MixAttributeFields.ReadViewModel> Fields { get; set; }

        [JsonProperty("data")]
        public JObject Obj { get; set; }

        [JsonProperty("nav")]
        public Navigation Nav
        {
            get
            {
                if (AttributeSetName == MixConstants.AttributeSetName.NAVIGATION && Obj != null)
                {
                    return Obj.ToObject<Navigation>();
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
            if (Obj == null)
            {

                Obj = new JObject();
                Values = Values ?? MixAttributeSetValues.NavigationViewModel
                    .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
                Obj.Add(new JProperty("id", Id));
                foreach (var item in Values.Where(m => m.DataType != MixEnums.MixDataType.Reference).OrderBy(v => v.Priority))
                {
                    if (!Obj.TryGetValue(item.AttributeFieldName, out JToken val))
                    {
                        var prop = ParseValue(item);
                        if (prop != null)
                        {
                            Obj.Add(prop);
                        }
                    }
                }
            }
            LoadReferenceData(Id, MixEnums.MixAttributeSetDataType.Set, _context, _transaction);
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



        #region Expands
        public void LoadReferenceData(string parentId, MixEnums.MixAttributeSetDataType parentType,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var refFields = Values.Where(m => m.DataType == MixEnums.MixDataType.Reference).OrderBy(v => v.Priority).ToList();
            foreach (var item in refFields)
            {
                item.Field = item.Field ?? MixAttributeFields.ReadViewModel.Repository.GetSingleModel(m => m.Id == item.AttributeFieldId
                , _context, _transaction).Data;
                Expression<Func<MixRelatedAttributeData, bool>> predicate = model =>
                    (model.AttributeSetId == item.Field.ReferenceId)
                    && (model.ParentId == parentId && model.ParentType == parentType.ToString())
                    && model.Specificulture == Specificulture
                    ;
                var getData = MixRelatedAttributeDatas.NavigationViewModel.Repository.GetModelListBy(predicate, _context, _transaction);

                JArray arr = new JArray();

                foreach (var nav in getData.Data.OrderBy(d => d.Priority))
                {
                    nav.Data.Obj.Add(new JProperty("data", nav.Data.Obj));
                    arr.Add(nav.Data.Obj);
                }
                if (Obj.ContainsKey(item.AttributeFieldName))
                {
                    Obj[item.AttributeFieldName] = arr;
                }
                else
                {
                    Obj.Add(new JProperty(item.AttributeFieldName, arr));
                }
            }
        }
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

                case MixEnums.MixDataType.Integer:
                    return (new JProperty(item.AttributeFieldName, item.IntegerValue));

                case MixEnums.MixDataType.Reference:
                    return null;

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

                case MixEnums.MixDataType.Integer:
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
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        [JsonProperty("actived_menu_items")]
        public List<MenuItem> ActivedMenuItems { get; set; } = new List<MenuItem>();

        [JsonProperty("actived_menu_item")]
        public MenuItem ActivedMenuItem { get; set; }
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