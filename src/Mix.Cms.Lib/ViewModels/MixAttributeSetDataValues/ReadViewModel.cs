using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetValues
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetValue, ReadViewModel>
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
        
        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
        
    }
}
