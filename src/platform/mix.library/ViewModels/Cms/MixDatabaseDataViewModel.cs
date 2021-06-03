using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Mix.Lib.Extensions;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixDatabaseDataViewModel : ViewModelBase<MixCmsContext, MixDatabaseData, MixDatabaseDataViewModel>
    {
        #region Properties

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        public JObject Obj { get; set; }

        #endregion Properties

        #region Contructors

        public MixDatabaseDataViewModel() : base()
        {
        }

        public MixDatabaseDataViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getValues = MixDatabaseDataValueViewModel
                   .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture, _context, _transaction);
           var properties = getValues.Data.Select(m => m.Model.ToJProperty());
            Obj = new JObject(
                new JProperty("id", Id),
                properties
            );
        }

        #endregion Overrides

        #region Expand

        #endregion Expand
    }
}
