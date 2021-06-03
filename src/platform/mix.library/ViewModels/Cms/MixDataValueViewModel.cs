using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixDataValueViewModel : ViewModelBase<MixCmsContext, MixDatabaseDataValue, MixDataValueViewModel>
    {
        #region Properties

        #region Models
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int MixDatabaseColumnId { get; set; }
        public string MixDatabaseColumnName { get; set; }
        public string MixDatabaseName { get; set; }
        public string Regex { get; set; }
        public MixDataType DataType { get; set; }
        public bool? BooleanValue { get; set; }
        public string DataId { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public int EncryptType { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        #endregion Models

        #endregion Properties

        #region Contructors

        public MixDataValueViewModel() : base()
        {
        }

        public MixDataValueViewModel(MixDatabaseDataValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
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
