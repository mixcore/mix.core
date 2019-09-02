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
    public class ReadMvcPostViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, ReadMvcPostViewModel>
    {
        #region Properties
        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("referrenceId")]
        public int? ReferenceId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Models
        #region Views
        [JsonProperty("postData")]
        public PaginationModel<MixPostAttributeDatas.ReadMvcViewModel> PostData { get; set; }
        #endregion
        #endregion Properties
        #region Contructors

        public ReadMvcPostViewModel() : base()
        {
        }

        public ReadMvcPostViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            PostData = MixPostAttributeDatas.ReadMvcViewModel.Repository.GetModelListBy(
                m => m.PostId == Id && m.Specificulture == Specificulture, "Priority", 0, null, null
                    , _context, _transaction).Data;
        }

        #endregion

        #region Expand
        public void LoadPostData(int postId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixPostAttributeDatas.ReadMvcViewModel.Repository
            .GetModelListBy(
                m => m.PostId == postId && m.Specificulture == specificulture && m.AttributeSetId == Id
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , pageSize, pageIndex
                , _context: _context, _transaction: _transaction);

            PostData = getData.Data;
        }

        #endregion
    }
}
