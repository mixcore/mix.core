using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeDatas
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixPostAttributeData, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("data")]
        public List<MixPostAttributeValues.UpdateViewModel> Data { get; set; }

        #endregion Views

        #endregion Properties

        public UpdateViewModel(MixPostAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(int attributeSetId, List<MixAttributeFields.UpdateViewModel> attributes)
        {
            AttributeSetId = attributeSetId;
            Specificulture = Specificulture;
            Data = new List<MixPostAttributeValues.UpdateViewModel>();
            foreach (var item in attributes)
            {
                Data.Add(new MixPostAttributeValues.UpdateViewModel()
                {
                    Specificulture = Specificulture,
                    AttributeName = item.Name,
                    DataType = item.DataType,
                    AttributeFieldId = item.Id
                });
            }
        }

        public UpdateViewModel(int attributeSetId)
        {
            AttributeSetId = attributeSetId;
            var attributes = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == attributeSetId).Data;
            Data = new List<MixPostAttributeValues.UpdateViewModel>();
            foreach (var item in attributes)
            {
                Data.Add(new MixPostAttributeValues.UpdateViewModel()
                {
                    Specificulture = Specificulture,
                    AttributeName = item.Name,
                    DataType = item.DataType,
                    AttributeFieldId = item.Id
                });
            }
        }

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                Data = MixPostAttributeValues.UpdateViewModel.Repository.GetModelListBy(
                    v => v.DataId == Id && v.Specificulture == Specificulture, _context, _transaction).Data;
            }
            else
            {
                var attributes = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == AttributeSetId).Data;
                Data = new List<MixPostAttributeValues.UpdateViewModel>();
                foreach (var item in attributes)
                {
                    Data.Add(new MixPostAttributeValues.UpdateViewModel()
                    {
                        Specificulture = Specificulture,
                        Priority = item.Priority,
                        AttributeName = item.Name,
                        DataType = item.DataType,
                        AttributeFieldId = item.Id,
                        Field = item
                    });
                }
            }
        }

        public override MixPostAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPostAttributeData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            //Save Data Fields
            foreach (var item in Data)
            {
                item.Specificulture = parent.Specificulture;
                item.DataId = parent.Id;
                item.PostId = parent.PostId;
                var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            return result;
        }

        #endregion Async

        #endregion overrides
    }
}