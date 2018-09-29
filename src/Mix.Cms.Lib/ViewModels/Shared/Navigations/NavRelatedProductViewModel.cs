using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.Shared
{
    public class NavRelatedProductViewModel
        : ViewModelBase<MixCmsContext, MixRelatedProduct, NavRelatedProductViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("sourceId")]
        public int SourceId { get; set; }

        [JsonProperty("destinationId")]
        public int DestinationId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("relatedProduct")]
        public Product.ReadMvcViewModel RelatedProduct { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public NavRelatedProductViewModel() : base()
        {
        }

        public NavRelatedProductViewModel(MixRelatedProduct model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getProduct = Lib.ViewModels.Product.ReadMvcViewModel.Repository.GetSingleModel(
                m => m.Id == DestinationId && m.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction);
            if (getProduct.IsSucceed)
            {
                this.RelatedProduct = getProduct.Data;
            }
        }

        public override MixRelatedProduct ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
