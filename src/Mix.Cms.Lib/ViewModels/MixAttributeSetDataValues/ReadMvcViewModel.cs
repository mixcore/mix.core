using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetValues
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetValue, ReadMvcViewModel>
    {
        #region Properties
        #region Models

        public string Id { get; set; }
        public int AttributeFieldId { get; set; }
        public string Regex { get; set; }
        public MixEnums.MixDataType DataType { get; set; }
        public int Status { get; set; }
        public string AttributeFieldName { get; set; }
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

        public List<MixRelatedAttributeDatas.ReadMvcViewModel> DataNavs { get; set; }

        #endregion
        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        #region Override
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (DataType == MixEnums.MixDataType.Reference)
            {
                DataNavs = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetModelListBy(d =>
                    d.ParentId == DataId && d.ParentType == (int)MixEnums.MixAttributeSetDataType.Set && d.Specificulture == Specificulture,
                _context, _transaction).Data;
            }
        }
        #endregion
    }
}
