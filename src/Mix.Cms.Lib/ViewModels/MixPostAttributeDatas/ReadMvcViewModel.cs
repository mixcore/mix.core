using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixPostAttributeDatas
{
    public class ReadMvcViewModel
       : ViewModelBase<MixCmsContext, MixPostAttributeData, ReadMvcViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("data")]
        public List<MixPostAttributeValues.ReadMvcViewModel> Data { get; set; }

        #endregion Views

        #endregion Properties

        public ReadMvcViewModel(MixPostAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadMvcViewModel() : base()
        {
        }

        #region overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Data = MixPostAttributeValues.ReadMvcViewModel.Repository.GetModelListBy(
                    v => v.DataId == Id && v.Specificulture == Specificulture, _context, _transaction).Data;
        }

        #endregion overrides

        #region Helper

        public string GetStringValue(string name)
        {
            return Data.FirstOrDefault(v => v.AttributeName == name)?.StringValue;
        }

        public DateTime? GetDateTimeValue(string name)
        {
            return Data.FirstOrDefault(v => v.AttributeName == name)?.DateTimeValue;
        }

        public bool? GetBooleanValue(string name)
        {
            return Data.FirstOrDefault(v => v.AttributeName == name)?.BooleanValue;
        }

        public int? GetIntegerValue(string name)
        {
            return Data.FirstOrDefault(v => v.AttributeName == name)?.IntegerValue;
        }

        public double? GetDoubleValue(string name)
        {
            return Data.FirstOrDefault(v => v.AttributeName == name)?.DoubleValue;
        }

        public string GetDecryptValue(string name)
        {
            return Data.FirstOrDefault(v => v.AttributeName == name)?.DecryptValue;
        }

        #endregion Helper
    }
}