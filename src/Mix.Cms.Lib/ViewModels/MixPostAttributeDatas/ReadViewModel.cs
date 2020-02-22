using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeDatas
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixPostAttributeData, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributeSetId")]
        public int SetAttributeId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        #endregion Models

        #endregion Properties

        public ReadViewModel(MixPostAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }
    }
}