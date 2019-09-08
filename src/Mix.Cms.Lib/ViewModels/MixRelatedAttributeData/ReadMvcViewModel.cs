using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using System;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, MixRelatedAttributeData, ReadMvcViewModel>
    {
        public ReadMvcViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadMvcViewModel() : base()
        {
        }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int ParentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        #region Views

        public MixAttributeSetDatas.ReadMvcViewModel Data { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixAttributeSetDatas.ReadMvcViewModel.Repository.GetSingleModel(p => p.Id == Id && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                Data = getData.Data;
            }
        }


        #region Async


        #endregion Async

        #endregion overrides
    }
}
