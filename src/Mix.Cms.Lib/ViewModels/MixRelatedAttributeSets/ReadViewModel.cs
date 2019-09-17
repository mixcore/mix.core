using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets
{
    public class ReadViewModel
        : ViewModelBase<MixCmsContext, MixRelatedAttributeSet, ReadViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ParentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("RelatedAttributeSet")]
        public MixAttributeSets.ReadViewModel RelatedAttributeSet { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixRelatedAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPost = MixAttributeSets.ReadViewModel.Repository.GetSingleModel(
                m => m.Id == Id
                , _context: _context, _transaction: _transaction);
            if (getPost.IsSucceed)
            {
                this.RelatedAttributeSet = getPost.Data;
            }
        }

        public override MixRelatedAttributeSet ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        #endregion Overrides
    }
}
