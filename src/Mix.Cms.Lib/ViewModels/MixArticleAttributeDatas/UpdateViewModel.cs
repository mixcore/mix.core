using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixArticleAttributeDatas
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixArticleAttributeData, UpdateViewModel>
    {
        #region Properties
        #region Models
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }
        [JsonProperty("articleId")]
        public int ArticleId { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        #endregion

        #region Views
        [JsonProperty("data")]
        public List<MixArticleAttributeValues.UpdateViewModel> Data{ get; set; }
        #endregion

        #endregion
        public UpdateViewModel(MixArticleAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
            Data = new List<MixArticleAttributeValues.UpdateViewModel>();
            foreach (var item in attributes)
            {
                Data.Add(new MixArticleAttributeValues.UpdateViewModel()
                {
                    Specificulture = Specificulture,
                    AttributeName = item.Name,
                    DataType = item.DataType
                });
            }
        }
        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Data = MixArticleAttributeValues.UpdateViewModel.Repository.GetModelListBy(
                    v => v.DataId == Id && v.Specificulture == Specificulture, _context, _transaction).Data;
        }
        public override MixArticleAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }
        #region Async
        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixArticleAttributeData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            //Save Data Fields
            foreach (var item in Data)
            {
                item.Specificulture = parent.Specificulture;
                item.DataId = parent.Id;
                item.ArticleId = parent.ArticleId;
                var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            return result;
        }
        #endregion Async

        #endregion overrides
    }
}
