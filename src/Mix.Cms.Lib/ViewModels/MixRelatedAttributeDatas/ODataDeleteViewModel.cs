using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using System;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class ODataDeleteViewModel
        : ODataViewModelBase<MixCmsContext, MixRelatedAttributeData, ODataDeleteViewModel>
    {
        #region Properties

        #region Models

        public string Id { get; set; }
        public string ParentId { get; set; }
        public int ParentType { get; set; }
        public int AttributeSetId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public ODataDeleteViewModel() : base()
        {
        }

        public ODataDeleteViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixRelatedAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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