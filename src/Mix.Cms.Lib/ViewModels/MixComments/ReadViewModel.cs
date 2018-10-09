using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixComments
{
    public class ReadViewModel: ViewModelBase<MixCmsContext, MixComment, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("customerId")]
        public string CreatedBy { get; set; }
        [JsonProperty("createdDate")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("rating")]
        public double Rating { get; set; }

        #endregion Models

        #region Views


        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixComment model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
    }
}
