using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDataValues
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseDataValue, ReadMvcViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("mixDatabaseColumnId")]
        public int MixDatabaseColumnId { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("mixDatabaseColumnName")]
        public string MixDatabaseColumnName { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

        [JsonProperty("booleanValue")]
        public bool? BooleanValue { get; set; }

        [JsonProperty("dataId")]
        public string DataId { get; set; }

        [JsonProperty("dateTimeValue")]
        public DateTime? DateTimeValue { get; set; }

        [JsonProperty("doubleValue")]
        public double? DoubleValue { get; set; }

        [JsonProperty("integerValue")]
        public int? IntegerValue { get; set; }

        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

        [JsonProperty("encryptValue")]
        public string EncryptValue { get; set; }

        [JsonProperty("encryptKey")]
        public string EncryptKey { get; set; }

        [JsonProperty("encryptType")]
        public int EncryptType { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        //[JsonProperty("dataNavs")]
        //public List<MixRelatedAttributeDatas.ReadMvcViewModel> DataNavs { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
            IsCache = false;
        }

        public ReadMvcViewModel(MixDatabaseDataValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
            IsCache = false;
        }

        #endregion Contructors

        #region Override

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //if (DataType == MixDataType.Reference)
            //{
            //    DataNavs = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetModelListBy(d =>
            //        d.DataId == DataId && d.Specificulture == Specificulture,
            //    _context, _transaction).Data;
            //}
        }

        #endregion Override
    }
}