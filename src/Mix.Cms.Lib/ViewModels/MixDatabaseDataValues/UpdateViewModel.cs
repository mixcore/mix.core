using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDataValues
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseDataValue, UpdateViewModel>
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

        [JsonProperty("editorValue")]
        public string EditorValue { get; set; }

        [JsonProperty("editorType")]
        public MixEditorType? EditorType { get; set; }

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

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("column")]
        public MixDatabaseColumns.UpdateViewModel Column { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
            //IsCache = false;
        }

        public UpdateViewModel(MixDatabaseDataValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
            //IsCache = false;
        }

        #endregion Contructors

        #region Overrides

        public override MixDatabaseDataValue ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
            }
            Priority = Column?.Priority ?? Priority;
            DataType = Column?.DataType ?? DataType;

            MixDatabaseColumnName = Column?.Name;
            MixDatabaseColumnId = Column?.Id ?? 0;
            if (string.IsNullOrEmpty(StringValue) && !string.IsNullOrEmpty(Column?.DefaultValue))
            {
                ParseDefaultValue(Column.DefaultValue);
            }
            return base.ParseModel(_context, _transaction);
        }

        private void ParseDefaultValue(string defaultValue)
        {
            StringValue = defaultValue;
            switch (DataType)
            {
                case MixDataType.DateTime:
                    break;

                case MixDataType.Date:
                    break;

                case MixDataType.Time:
                    break;

                case MixDataType.Double:
                    double.TryParse(defaultValue, out double doubleValue);
                    DoubleValue = DoubleValue;
                    break;

                case MixDataType.Boolean:
                    bool.TryParse(defaultValue, out bool boolValue);
                    BooleanValue = boolValue;
                    break;

                case MixDataType.Integer:
                    int.TryParse(defaultValue, out int intValue);
                    IntegerValue = intValue;
                    break;
            }
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (DataType == MixDataType.Html)
            {
                EditorValue ??= StringValue;
                EditorType ??= MixEditorType.Html;
            }
            
            if (string.IsNullOrEmpty(Id))
            {
                Status = Status == default ? Enum.Parse<MixContentStatus>(MixService.GetAppSetting<string>
                    (MixAppSettingKeywords.DefaultContentStatus)) : Status;
            }

            if (MixDatabaseColumnId > 0)
            {
                Column = MixDatabaseColumns.UpdateViewModel.Repository.GetSingleModel(
                    f => f.Id == MixDatabaseColumnId
                    , _context, _transaction).Data;
                if (Column != null && DataType == MixDataType.Reference)
                {
                    MixDatabaseName = _context.MixDatabase.FirstOrDefault(m => m.Id == Column.ReferenceId)?.Name;
                }
            }
            else // additional field for page / post / module => id = 0
            {
                Column = new MixDatabaseColumns.UpdateViewModel()
                {
                    DataType = DataType,
                    Title = MixDatabaseColumnName,
                    Name = MixDatabaseColumnName,
                    Priority = Priority
                };
            }

            if (string.IsNullOrEmpty(Id) && Column != null)
            {
                ParseDefaultValue(Column.DefaultValue);
            }
        }

        #endregion Overrides

        #region Expands

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid && Column != null)
            {
                if (Column.IsUnique)
                {
                    var exist = _context.MixDatabaseDataValue.Any(d => d.Specificulture == Specificulture
                        && d.MixDatabaseName == MixDatabaseName
                        && EF.Functions.Like(d.StringValue, StringValue) && d.Id != Id && d.DataId != DataId);
                    if (exist)
                    {
                        IsValid = false;
                        Errors.Add($"{DataId}: {Column.Title} = {StringValue} is existed");
                    }
                }
                if (Column.IsRequire)
                {
                    if (string.IsNullOrEmpty(StringValue))
                    {
                        IsValid = false;
                        Errors.Add($"{DataId}: {Column.Title} is required");
                    }
                }
                if (!string.IsNullOrEmpty(Column.Regex))
                {
                    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(Column.Regex, RegexOptions.IgnoreCase);
                    Match m = r.Match(StringValue);
                    if (!m.Success)
                    {
                        IsValid = false;
                        Errors.Add($"{DataId}: {Column.Title} is invalid");
                    }
                }
            }
        }

        #endregion Expands
    }
}