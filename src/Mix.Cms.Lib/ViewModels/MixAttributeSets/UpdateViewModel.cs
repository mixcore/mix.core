using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, UpdateViewModel>
    {
        #region Properties
        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("type")]
        public int? Type { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }

        #endregion Models
        #region Views
        [JsonProperty("attributes")]
        public List<MixAttributeFields.UpdateViewModel> Attributes { get; set; }
        [JsonProperty("removeAttributes")]
        public List<MixAttributeFields.UpdateViewModel> RemoveAttributes { get; set; } = new List<MixAttributeFields.UpdateViewModel>();

        [JsonProperty("articleData")]
        public PaginationModel<MixPostAttributeDatas.UpdateViewModel> PostData { get;set;}
        #endregion
        #endregion Properties
        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Attributes = MixAttributeFields.UpdateViewModel
                .Repository.GetModelListBy(a => a.AttributeSetId == Id, _context, _transaction).Data.OrderBy(a=>a.Priority).ToList();
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

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSet parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Attributes)
                {
                    if (result.IsSucceed)
                    {
                        item.AttributeSetId = parent.Id;
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (result.IsSucceed)
            {
                foreach (var item in RemoveAttributes)
                {
                    if (result.IsSucceed)
                    {
                        var removeResult = await item.RemoveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(removeResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public override  RepositoryResponse<bool> SaveSubModels(MixAttributeSet parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Attributes)
                {
                    if (result.IsSucceed)
                    {
                        item.AttributeSetId = parent.Id;
                        var saveResult =  item.SaveModel(false, _context, _transaction);
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

        #region Expand
        public void LoadPostData(int articleId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixPostAttributeDatas.UpdateViewModel.Repository
            .GetModelListBy(
                m => m.PostId == articleId && m.Specificulture == specificulture
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , pageSize, pageIndex
                , _context: _context, _transaction: _transaction);
            if (!getData.IsSucceed || getData.Data == null || PostData.TotalItems == 0)
            {
                PostData = new PaginationModel<MixPostAttributeDatas.UpdateViewModel>() { TotalItems = 1};
                PostData.Items.Add(new MixPostAttributeDatas.UpdateViewModel(Id, Attributes));
            }
        }
        #endregion
    }
}
