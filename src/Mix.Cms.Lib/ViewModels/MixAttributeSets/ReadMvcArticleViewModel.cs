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
    public class ReadMvcArticleViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, ReadMvcArticleViewModel>
    {
        #region Properties
        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Models
        #region Views
        [JsonProperty("articleData")]
        public PaginationModel<MixArticleAttributeDatas.ReadMvcViewModel> ArticleData { get; set; }
        #endregion
        #endregion Properties
        #region Contructors

        public ReadMvcArticleViewModel() : base()
        {
        }

        public ReadMvcArticleViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            ArticleData = MixArticleAttributeDatas.ReadMvcViewModel.Repository.GetModelListBy(
                m => m.ArticleId == Id && m.Specificulture == Specificulture, "Priority", 0, null, null
                    , _context, _transaction).Data;
        }

        #endregion

        #region Expand
        public void LoadArticleData(int articleId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixArticleAttributeDatas.ReadMvcViewModel.Repository
            .GetModelListBy(
                m => m.ArticleId == articleId && m.Specificulture == specificulture && m.AttributeSetId == Id
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , pageSize, pageIndex
                , _context: _context, _transaction: _transaction);

            ArticleData = getData.Data;
        }
        #endregion
    }
}
