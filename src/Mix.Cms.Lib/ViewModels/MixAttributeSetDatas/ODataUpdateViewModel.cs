using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ODataUpdateViewModel
      : ODataViewModelBase<MixCmsContext, MixAttributeSetData, ODataUpdateViewModel>
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
        //[JsonProperty("dataNavs")]
        //public List<MixRelatedAttributeDatas.UpdateViewModel> DataNavs { get; set; }

        #endregion
        #endregion Properties

        #region Contructors

        public ODataUpdateViewModel() : base()
        {
        }

        public ODataUpdateViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {            
            // Related Datas
            //DataNavs = MixRelatedAttributeDatas.UpdateViewModel.Repository.GetModelListBy(
            //    n => n.ParentId == Id && n.ParentType == (int)MixEnums.MixAttributeSetDataType.Set && n.Specificulture == Specificulture,
            //    _context, _transaction).Data;

            Values = MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
            Fields = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId, _context, _transaction).Data;
            foreach (var field in Fields.OrderBy(f=>f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                if (val == null)
                {
                    val = new MixAttributeSetValues.UpdateViewModel(
                        new MixAttributeSetValue() { AttributeFieldId = field.Id }
                        , _context, _transaction);
                    val.Field = field;
                    val.AttributeFieldName = field.Name;
                    val.StringValue = field.DefaultValue;
                    val.Priority = field.Priority;
                    Values.Add(val);
                }
                val.AttributeSetName = AttributeSetName;
                val.Priority = field.Priority;
                val.Field = field;
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
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        item.Priority = item.Field.Priority;
                        item.DataId = parent.Id;
                        item.Specificulture = parent.Specificulture;
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
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

        public override RepositoryResponse<bool> SaveSubModels(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        item.Priority = item.Field.Priority;
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
        #endregion

    }
}
