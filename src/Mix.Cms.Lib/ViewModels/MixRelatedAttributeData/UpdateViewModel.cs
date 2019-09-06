using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixRelatedAttributeData, UpdateViewModel>
    {
        public UpdateViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public UpdateViewModel() : base()
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

        public MixAttributeSetDatas.UpdateViewModel Data { get; set; }

        #endregion Views

        #region overrides

        public override MixRelatedAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixAttributeSetDatas.UpdateViewModel.Repository.GetSingleModel(p => p.Id == Id && p.Specificulture == Specificulture
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
