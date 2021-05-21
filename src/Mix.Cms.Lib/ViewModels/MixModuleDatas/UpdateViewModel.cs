using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixModuleDatas
{
    public class UpdateViewModel : ViewModelBase<MixCmsContext, MixModuleData, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("moduleId")]
        public int ModuleId { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; } = "[]";

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("postId")]
        public int? PostId { get; set; }

        [JsonProperty("productId")]
        public int? ProductId { get; set; }

        [JsonProperty("pageId")]
        public int? PageId { get; set; }

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

        [JsonProperty("dataProperties")]
        public List<ApiModuleDataValueViewModel> DataProperties { get; set; }

        [JsonProperty("jItem")]
        public JObject JItem { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixModuleData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixModuleData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            LastModified = DateTime.UtcNow;
            Value = JsonConvert.SerializeObject(JItem);
            Fields = JsonConvert.SerializeObject(DataProperties);
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Fields = _context.MixModule.First(m => m.Id == ModuleId && m.Specificulture == Specificulture)?.Fields;
            DataProperties = Fields == null ? null : JsonConvert.DeserializeObject<List<ApiModuleDataValueViewModel>>(Fields);
            JItem = Value == null ? InitValue() : JsonConvert.DeserializeObject<JObject>(Value);
            foreach (var item in DataProperties)
            {
                JItem[item.Name] = Helper.ParseValue(JItem, item);
                if (JItem[item.Name] == null)
                {
                    JItem[item.Name] = new JObject()
                    {
                        new JProperty("dataType", item.DataType),
                        new JProperty("value", JItem[item.Name]?.Value<JObject>().Value<string>("value"))
                    };
                }
            }
        }

        public override void Validate(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            base.Validate(_context, _transaction);

            if (IsValid)
            {
                foreach (var col in DataProperties)
                {
                    var result = col.Validate<MixModuleData>(Id, Specificulture, JItem, _context, _transaction);
                    if (!result.IsSucceed)
                    {
                        IsValid = false;
                        Errors.AddRange(result.Errors);
                    }
                }
            }
        }

        #endregion Overrides

        #region Expands

        public string ParseObjectValue()
        {
            JObject result = new JObject();
            foreach (var prop in DataProperties)
            {
                JObject obj = new JObject();
                obj.Add(new JProperty("dataType", prop.DataType));
                obj.Add(new JProperty("value", prop.Value));
                result.Add(new JProperty(MixCommonHelper.ParseJsonPropertyName(prop.Name), obj));
            }
            return result.ToString(Formatting.None);
        }

        public JObject InitValue()
        {
            JObject result = new JObject();
            foreach (var prop in DataProperties)
            {
                JObject obj = new JObject();
                obj.Add(new JProperty("dataType", prop.DataType));
                obj.Add(new JProperty("value", prop.Value));
                result.Add(new JProperty(MixCommonHelper.ParseJsonPropertyName(prop.Name), obj));
            }
            return result;
        }

        public string GetStringValue(string name)
        {
            var prop = DataProperties.FirstOrDefault(p => p.Name == name);
            return prop != null && prop.Value != null ? prop.Value.ToString() : string.Empty;
        }

        public ApiModuleDataValueViewModel GetDataProperty(string name)
        {
            return DataProperties.FirstOrDefault(p => p.Name == name);
        }

        #endregion Expands
    }
}