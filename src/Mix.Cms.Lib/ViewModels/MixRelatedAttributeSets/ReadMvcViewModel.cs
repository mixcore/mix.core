using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using System;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, MixRelatedAttributeSet, ReadMvcViewModel>
    {
        public ReadMvcViewModel(MixRelatedAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadMvcViewModel() : base()
        {
        }

        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ParentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        #region Views

        public MixAttributeSets.ReadMvcViewModel AttributeSet { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixAttributeSets.ReadMvcViewModel.Repository.GetSingleModel(p => p.Id == Id
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                AttributeSet = getData.Data;
            }
        }

        #endregion overrides
    }
}