using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, ReadViewModel>
    {
        #region Properties
        #region Models

        public string Id { get; set; }
        public int AttributeSetId { get; set; }
        public int ModuleId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Status { get; set; }

        #endregion Models
        #region Views

        [JsonProperty("values")]
        public List<MixAttributeSetValues.UpdateViewModel> Values { get; set; }

        #endregion
        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Values = MixAttributeSetValues.UpdateViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction).Data.OrderBy(a => a.Priority).ToList();

        }

        #endregion
    }
}
