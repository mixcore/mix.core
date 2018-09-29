using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.Shared.Info;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Cms.Lib.ViewModels.Shared
{
    public class NavModuleArticleViewModel
       : ViewModelBase<MixCmsContext, MixModuleArticle, NavModuleArticleViewModel>
    {
        public NavModuleArticleViewModel(MixModuleArticle model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public NavModuleArticleViewModel() : base()
        {
        }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("ModuleId")]
        public int ModuleId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views

        public InfoArticleViewModel Article { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getArticle = InfoArticleViewModel.Repository.GetSingleModel(p => p.Id == ArticleId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getArticle.IsSucceed)
            {
                Article = getArticle.Data;
            }
        }

        #region Async

        #endregion Async

        #endregion overrides
    }
}
