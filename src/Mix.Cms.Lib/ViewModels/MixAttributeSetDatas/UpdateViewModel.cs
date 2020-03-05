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
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, UpdateViewModel>
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

        [JsonProperty("values")]
        public List<MixAttributeSetValues.UpdateViewModel> Values { get; set; }

        [JsonProperty("fields")]
        public List<MixAttributeFields.UpdateViewModel> Fields { get; set; }

        [JsonProperty("dataNavs")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> DataNavs { get; set; }

        [JsonProperty("data")]
        public JObject Data { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            // Related Datas
            DataNavs = MixRelatedAttributeDatas.UpdateViewModel.Repository.GetModelListBy(
                n => n.ParentId == Id && n.ParentType == (int)MixEnums.MixAttributeSetDataType.Set && n.Specificulture == Specificulture,
                _context, _transaction).Data;

            Values = MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            Fields = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => (f.AttributeSetId == AttributeSetId || f.AttributeSetName == AttributeSetName), _context, _transaction).Data;
            foreach (var field in Fields.OrderBy(f => f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                if (val == null)
                {
                    val = new MixAttributeSetValues.UpdateViewModel(
                        new MixAttributeSetValue() { AttributeFieldId = field.Id }
                        , _context, _transaction)
                    {
                        Field = field,
                        AttributeFieldName = field.Name,
                        StringValue = field.DefaultValue,
                        Priority = field.Priority
                    };
                    Values.Add(val);
                }
                val.AttributeSetName = AttributeSetName;
                val.Priority = field.Priority;
                val.Field = field;
                val.DataType = val.Field.DataType;
                val.AttributeFieldName = val.AttributeFieldName ?? val.Field?.Name;
            }
        }

        public override MixAttributeSetData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                // TODO: Double check logic code
                var addictionalSet = _context.MixAttributeSet.FirstOrDefault(m => m.Name == "sys_additional_field");
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        if (addictionalSet != null && item.Field != null && item.Field.Id == 0)
                        {
                            // Add field to addictional_field set
                            item.Field.AttributeSetId = addictionalSet.Id;
                            item.Field.AttributeSetName = addictionalSet.Name;
                            var saveField = await item.Field.SaveModelAsync(false, _context, _transaction);
                            ViewModelHelper.HandleResult(saveField, ref result);
                        }
                        if (result.IsSucceed)
                        {
                            item.AttributeFieldId = item.Field.Id;
                            item.AttributeFieldName = item.Field.Name;
                            item.Priority = item.Field?.Priority ?? item.Priority;
                            item.DataId = parent.Id;
                            item.Specificulture = parent.Specificulture;
                            var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
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

        public override RepositoryResponse<bool> SaveSubModels(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        item.Field = Fields.Find(f => f.Name == item.AttributeFieldName);
                        item.Priority = item.Field?.Priority ?? item.Priority;
                        item.DataId = parent.Id;
                        item.Specificulture = parent.Specificulture;
                        var saveResult = item.SaveModel(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        //public override List<Task> GenerateRelatedData(MixCmsContext context, IDbContextTransaction transaction)
        //{
        //    var tasks = new List<Task>();

        //    // Remove parent caches

        //    // 1. Remove Parent Attribute Data
        //    var attrDatas = context.MixAttributeSetData.Where(m => m.MixRelatedAttributeData
        //    .Any(d => d.Specificulture == Specificulture && d.Id == Id && d.ParentType == (int)MixEnums.MixAttributeSetDataType.Set));
        //    foreach (var item in attrDatas)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            UpdateViewModel.Repository.RemoveCache(item, context, transaction);
        //        }));
        //    }
        //    return tasks;
        //}

        #endregion Overrides

        #region Expand

        private JProperty ParseValue(MixAttributeSetValues.UpdateViewModel item)
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
                    //string url = $"/api/v1/odata/en-us/related-attribute-set-data/mobile/parent/set/{Id}/{item.Field.ReferenceId}";
                    return (new JProperty(item.AttributeFieldName, new JArray()));

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

        private void ParseData()
        {
            Data = new JObject
            {
                new JProperty("id", Id),
                new JProperty("specificulture", Specificulture),
                new JProperty("createdDateTime", CreatedDateTime)
            };
            foreach (var item in Values.OrderBy(v => v.Priority))
            {
                Data.Add(ParseValue(item));
            }
        }

        #endregion Expand
    }
}