using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using System;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetValues
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetValue, UpdateViewModel>
    {
        #region Properties
        #region Models

        public string Id { get; set; }
        public int AttributeFieldId { get; set; }
        public string Regex { get; set; }
        public int DataType { get; set; }
        public int Status { get; set; }
        public string AttributeName { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string DataId { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public string EncryptValue { get; set; }
        public string EncryptKey { get; set; }
        public int EncryptType { get; set; }


        #endregion Models

        #region Views        
        public MixAttributeFields.UpdateViewModel Field { get; set; }
        #endregion

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Overrides
        public override MixAttributeSetValue ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            Priority = Field.Priority;
            DataType = Field.DataType;

            return base.ParseModel(_context, _transaction);
        }
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Field = MixAttributeFields.UpdateViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId).Data;
        }
        #endregion

        #region Expands

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (Field.IsUnique)
                {
                    var exist = _context.MixAttributeSetValue.Any(d => d.Specificulture == Specificulture
                        && d.StringValue == StringValue && d.Id != Id);
                    IsValid = false;
                    Errors.Add($"{Field.Title} is existed");
                }
                if (Field.IsRequire)
                {
                    if (string.IsNullOrEmpty(StringValue))
                    {
                        IsValid = false;
                        Errors.Add($"{Field.Title} is required");
                    }
                }
            }
        }

        #endregion
    }
}
