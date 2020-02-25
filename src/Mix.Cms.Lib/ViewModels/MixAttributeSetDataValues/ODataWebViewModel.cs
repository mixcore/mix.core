using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetValues
{
    public class ODataWebViewModel
      : ODataViewModelBase<MixCmsContext, MixAttributeSetValue, ODataWebViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributeFieldId")]
        public int AttributeFieldId { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("dataType")]
        public MixEnums.MixDataType DataType { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("attributeFieldName")]
        public string AttributeFieldName { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("booleanValue")]
        public bool? BooleanValue { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

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

        #endregion Models

        #region Views

        [JsonProperty("field")]
        public MixAttributeFields.ODataWebViewModel Field { get; set; }

        [JsonProperty("dataNavs")]
        public List<MixRelatedAttributeDatas.ODataWebViewModel> DataNavs { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ODataWebViewModel() : base()
        {
            IsCache = false;
        }

        public ODataWebViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
            IsCache = false;
        }

        #endregion Contructors

        #region Override

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Field = MixAttributeFields.ODataWebViewModel.Repository.GetSingleModel(f => f.Id == AttributeFieldId, _context, _transaction).Data;
            if (DataType == MixEnums.MixDataType.Reference)
            {
                DataNavs = MixRelatedAttributeDatas.ODataWebViewModel.Repository.GetModelListBy(d =>
                    d.AttributeSetId == Field.ReferenceId && d.ParentId == DataId && d.ParentType == (int)MixEnums.MixAttributeSetDataType.Set && d.Specificulture == Specificulture,
                _context, _transaction).Data;
            }
        }

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

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (Field.IsUnique)
                {
                    var exist = _context.MixAttributeSetValue.Any(d => d.Specificulture == Specificulture
                        && d.StringValue == StringValue && d.Id != Id && d.DataId != DataId);
                    if (exist)
                    {
                        IsValid = false;
                        Errors.Add($"{Field.Title} = {StringValue} is existed");
                    }
                }
                if (Field.IsRequire)
                {
                    if (string.IsNullOrEmpty(StringValue))
                    {
                        IsValid = false;
                        Errors.Add($"{Field.Title} is required");
                    }
                }
                if (!string.IsNullOrEmpty(Field.Regex))
                {
                    if (System.Text.RegularExpressions.Regex.Match(StringValue, Regex) == null)
                    {
                        IsValid = false;
                        Errors.Add($"{Field.Title} is invalid");
                    }
                }
            }
        }

        #endregion Override
    }
}