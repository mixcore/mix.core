using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixModuleDatas
{
    public class ImportViewModel
      : ViewModelBase<MixCmsContext, MixModuleData, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("moduleId")]
        public int ModuleId { get; set; }

        [JsonIgnore]
        [JsonProperty("fields")]
        public string Fields { get; set; } = "[]";

        [JsonProperty("value")]
        [JsonIgnore]
        public string Value { get; set; }

        [JsonProperty("postId")]
        public int? PostId { get; set; }

        [JsonProperty("productId")]
        public int? ProductId { get; set; }

        [JsonProperty("pageId")]
        public int? PageId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("updatedDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("dataProperties")]
        public List<ApiModuleDataValueViewModel> DataProperties { get; set; }

        [JsonProperty("jItem")]
        public JObject JItem { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixModuleData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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
            else
            {
                UpdatedDateTime = DateTime.UtcNow;
            }
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
                if (!JItem.TryGetValue(item.Name, out JToken tmp))
                {
                    string val = string.Empty;
                    switch (item.DataType)
                    {
                        case MixEnums.MixDataType.Upload:
                            val = Path.Combine(MixService.GetConfig<string>("Domain"), JItem[item.Name]?.Value<JObject>().Value<string>("value"));
                            break;

                        default:
                            val = JItem[item.Name]?.Value<JObject>().Value<string>("value");
                            break;
                    }
                    JItem[item.Name] = new JObject()
                    {
                        new JProperty("dataType", item.DataType),
                        new JProperty("value", val)
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
                result.Add(new JProperty(prop.Name, obj));
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
                result.Add(new JProperty(prop.Name, obj));
            }
            return result;
        }

        public string GetStringValue(string name)
        {
            var prop = DataProperties.FirstOrDefault(p => p.Name == name);
            return prop != null && prop.Value != null ? prop.Value.ToString() : string.Empty;
        }

        public string Property(string name)
        {
            return JItem[name]?.Value<JObject>().Value<string>("value");
        }

        public ApiModuleDataValueViewModel GetDataProperty(string name)
        {
            return DataProperties.FirstOrDefault(p => p.Name == name);
        }

        public static async System.Threading.Tasks.Task<RepositoryResponse<List<ReadViewModel>>> UpdateInfosAsync(List<ReadViewModel> data)
        {
            MixCmsContext context = new MixCmsContext();
            var transaction = context.Database.BeginTransaction();
            var result = new RepositoryResponse<List<ReadViewModel>>();
            try
            {
                foreach (var item in data)
                {
                    var model = context.MixModuleData.FirstOrDefault(m => m.Id == item.Id && m.Specificulture == item.Specificulture);
                    if (model != null)
                    {
                        model.Priority = item.Priority;
                        context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                }
                result.IsSucceed = (await context.SaveChangesAsync()) > 0;
                if (!result.IsSucceed)
                {
                    result.Errors.Add("Nothing changed");
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, true, transaction);
                return result;
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<ReadViewModel>(ex, true, transaction);
                return new RepositoryResponse<List<ReadViewModel>>()
                {
                    IsSucceed = false,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                //if current Context is Root
                transaction.Dispose();
                context.Dispose();
            }

            #endregion Expands
        }
    }
}