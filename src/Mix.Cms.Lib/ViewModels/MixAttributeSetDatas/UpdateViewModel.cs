﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, UpdateViewModel>
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
        [JsonProperty("relatedData")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> RelatedData { get; set; } = new List<MixRelatedAttributeDatas.UpdateViewModel>();

        [JsonProperty("values")]
        public List<MixAttributeSetValues.UpdateViewModel> Values { get; set; }

        [JsonProperty("fields")]
        public List<MixAttributeFields.UpdateViewModel> Fields { get; set; }

        [JsonProperty("dataNavs")]
        public List<MixRelatedAttributeDatas.UpdateViewModel> DataNavs { get; set; }

        [JsonProperty("data")]
        public JObject Data { get; set; }
        [JsonProperty("parentId")]
        public string ParentId { get; set; }
        [JsonProperty("parentType")]
        public MixDatabaseContentAssociationType ParentType { get; set; }

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
            if (string.IsNullOrEmpty(Id))
            {
                Status = Status == default ? Enum.Parse<MixContentStatus>(MixService.GetConfig<string>(AppSettingKeywords.DefaultContentStatus)) : Status;
            }
            // Related Datas
            DataNavs = MixRelatedAttributeDatas.UpdateViewModel.Repository.GetModelListBy(
                n => n.ParentId == Id && n.ParentType == MixDatabaseContentAssociationType.DataData && n.Specificulture == Specificulture,
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
                if (AttributeSetId == 0 && !string.IsNullOrEmpty(AttributeSetName))
                {
                    AttributeSetId = MixAttributeSets.ReadViewModel.Repository.GetSingleModel(m => m.Name == AttributeSetName, _context, _transaction).Data?.Id ?? 0;
                }
                else if (AttributeSetId > 0 && string.IsNullOrEmpty(AttributeSetName))
                {
                    AttributeSetName = MixAttributeSets.ReadViewModel.Repository.GetSingleModel(m => m.Name == AttributeSetName, _context, _transaction).Data?.Name;
                }
            }
            return base.ParseModel(_context, _transaction);
        }
        public override async Task<RepositoryResponse<UpdateViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = await base.SaveModelAsync(isSaveSubModels, context, transaction);
                // if save current data success and there is related parent data
                if (result.IsSucceed && !string.IsNullOrEmpty(ParentId))
                {
                    MixRelatedAttributeDatas.UpdateViewModel nav = new MixRelatedAttributeDatas.UpdateViewModel()
                    {
                        DataId = result.Data.Id,
                        Specificulture = Specificulture,
                        AttributeSetId = result.Data.AttributeSetId,
                        AttributeSetName = result.Data.AttributeSetName,
                        ParentId = ParentId,
                        ParentType = ParentType
                    };
                    var saveNav = await nav.SaveModelAsync(true, context, transaction);
                    result.IsSucceed = result.IsSucceed && saveNav.IsSucceed;
                    result.Errors = saveNav.Errors;
                    result.Exception = saveNav.Exception;
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<UpdateViewModel>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }

            }
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
                    if (item.DataId != parent.Id)
                    {
                        item.Id = null;
                    }
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
                            item.AttributeFieldId = item.Field?.Id ?? item.AttributeFieldId;
                            item.AttributeFieldName = item.Field?.Name ?? item.AttributeFieldName;
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


        #endregion Overrides

        #region Expand
        private async Task<RepositoryResponse<bool>> SaveRelatedDataAsync(MixAttributeSetData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            foreach (var item in RelatedData)
            {
                if (result.IsSucceed)
                {
                    if (string.IsNullOrEmpty(item.ParentId) && item.ParentType == MixDatabaseContentAssociationType.DataData)
                    {
                        var set = context.MixAttributeSet.First(s => s.Name == item.ParentName);
                        item.ParentId = set.Id.ToString();
                    }
                    item.Specificulture = Specificulture;
                    item.AttributeSetId = parent.AttributeSetId;
                    item.AttributeSetName = parent.AttributeSetName;
                    item.Id = parent.Id;
                    item.CreatedDateTime = DateTime.UtcNow;
                    var saveResult = await item.SaveModelAsync(true, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
                }
            }

            return result;
        }
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