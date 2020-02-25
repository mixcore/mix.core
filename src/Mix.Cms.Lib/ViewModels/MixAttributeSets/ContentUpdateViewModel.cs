using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class ContentUpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, ContentUpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ReferenceId")]
        public int? ReferenceId { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("formTemplate")]
        public string FormTemplate { get; set; }

        [JsonProperty("edmTemplate")]
        public string EdmTemplate { get; set; }

        [JsonProperty("edmSubject")]
        public string EdmSubject { get; set; }

        [JsonProperty("edmFrom")]
        public string EdmFrom { get; set; }

        [JsonProperty("edmAutoSend")]
        public bool? EdmAutoSend { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("attributes")]
        public List<MixAttributeFields.UpdateViewModel> Attributes { get; set; }

        [JsonProperty("postData")]
        public PaginationModel<MixRelatedAttributeDatas.UpdateViewModel> PostData { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ContentUpdateViewModel() : base()
        {
        }

        public ContentUpdateViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Attributes = MixAttributeFields.UpdateViewModel
                .Repository.GetModelListBy(a => a.AttributeSetId == Id, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();
        }

        public override MixAttributeSet ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        #endregion Overrides

        #region Expand

        public void LoadPostData(int postId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixRelatedAttributeDatas.UpdateViewModel.Repository
            .GetModelListBy(
                m => m.ParentId == postId.ToString() && m.ParentType == (int)MixEnums.MixAttributeSetDataType.Post && m.Specificulture == specificulture
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , pageSize, pageIndex
                , _context: _context, _transaction: _transaction);
            if (!getData.IsSucceed || getData.Data == null || getData.Data.Items.Count == 0)
            {
                PostData = new PaginationModel<MixRelatedAttributeDatas.UpdateViewModel>() { TotalItems = 1 };
                //PostData.Items.Add(new MixPostAttributeDatas.UpdateViewModel(Id, Attributes));
            }
            else
            {
                PostData = getData.Data;
            }
        }

        #endregion Expand
    }
}