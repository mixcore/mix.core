using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.Abstracts.ViewModels
{
    public class MixPostViewModelBase<T> : ViewModelBase<MixCmsContext, MixPost, T>
        where T: ViewModelBase<MixCmsContext, MixPost, T>
    {
        #region Properties

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Content { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public string Excerpt { get; set; }
        public string ExtraProperties { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public string Source { get; set; }
        public string Tags { get; set; }
        public string Template { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int? Views { get; set; }
        public string ExtraFields { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Properties

        #region Contructors

        public MixPostViewModelBase() : base()
        {
        }

        public MixPostViewModelBase(MixPost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #endregion Overrides

        #region Expand

        #endregion Expand
    }
}
