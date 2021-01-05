using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, ReadMvcViewModel>
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
        [JsonProperty("values")]
        public List<MixAttributeSetValues.ReadViewModel> Values { get; set; }

        [JsonProperty("obj")]
        public JObject Obj { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { 
            get => !string.IsNullOrEmpty(Id) && HasValue("seo_url") 
                    ? $"/data/{Specificulture}/{AttributeSetName}/{Property<string>("seo_url")}" 
                    : null; 
        }

        [JsonProperty("templatePath")]
        public string TemplatePath { get => $"{MixCmsHelper.GetTemplateFolder(Specificulture)}/{Property<string>("template_path")}"; }

        #endregion Views

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
            if (Obj == null)
            {
                Obj = new JObject();
                Values = Values ?? MixAttributeSetValues.ReadViewModel
                    .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
                Obj.Add(new JProperty("id", Id));
                foreach (var item in Values.Where(m => m.DataType != MixEnums.MixDataType.Reference).OrderBy(v => v.Priority))
                {
                    item.Field = item.Field ?? MixAttributeFields.ReadViewModel.Repository.GetSingleModel(m => m.Id == item.AttributeFieldId, _context, _transaction).Data;
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

        #endregion Overrides

        #region Expands
        public bool HasValue(string fieldName)
        {
            return Obj.Value<string>(fieldName) != null;
        }
        
        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(Obj, fieldName);
        }

        private JProperty ParseValue(MixAttributeSetValues.ReadViewModel item)
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
                    return (new JProperty(item.AttributeFieldName, null));

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
                var getData = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetModelListBy(predicate, _context, _transaction);

                JArray arr = new JArray();
                foreach (var nav in getData.Data.OrderBy(v => v.Priority))
                {
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
        #endregion Expands
    }
}