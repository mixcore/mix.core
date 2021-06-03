using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixLanguageViewModel : ViewModelBase<MixCmsContext, MixLanguage, MixLanguageViewModel>
    {
        #region Properties

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Keyword { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Properties

        #region Contructors

        public MixLanguageViewModel() : base()
        {
        }

        public MixLanguageViewModel(MixLanguage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
