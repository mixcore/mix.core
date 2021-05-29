using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Enums;
using Mix.Lib.Entities.Cms;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Mix.Lib.Abstracts.ViewModels;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportMixDataValueViewModel
      : MixDatabaseDataValuesBase<ImportMixDataValueViewModel, ImportaMixDatabaseColumnViewModel>
    {
        #region Properties
        
        #endregion Properties

        #region Contructors

        public ImportMixDataValueViewModel() : base()
        {
            //IsCache = false;
        }

        public ImportMixDataValueViewModel(MixDatabaseDataValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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
            if (MixDatabaseColumnId > 0)
            {
                Column = ImportaMixDatabaseColumnViewModel.Repository.GetSingleModel(f => f.Id == MixDatabaseColumnId).Data;
                if (Column != null && DataType == MixDataType.Reference)
                {
                    MixDatabaseName = _context.MixDatabase.FirstOrDefault(m => m.Id == Column.ReferenceId)?.Name;
                }
            }
            else // additional field for page / post / module => id = 0
            {
                Column = new ImportaMixDatabaseColumnViewModel()
                {
                    DataType = DataType,
                    Id = MixDatabaseColumnId,
                    Title = MixDatabaseColumnName,
                    Name = MixDatabaseColumnName,
                    Priority = Priority
                };
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
                        && EF.Functions.Like(d.StringValue, StringValue) && d.Id != Id && d.DataId != DataId);
                    if (exist)
                    {
                        IsValid = false;
                        Errors.Add($"{Column.Title} = {StringValue} is existed");
                    }
                }
                if (Column.IsRequire)
                {
                    if (string.IsNullOrEmpty(StringValue))
                    {
                        IsValid = false;
                        Errors.Add($"{Column.Title} is required");
                    }
                }
                if (!string.IsNullOrEmpty(Column.Regex))
                {
                    System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(Column.Regex, RegexOptions.IgnoreCase);
                    Match m = r.Match(StringValue);
                    if (!m.Success)
                    {
                        IsValid = false;
                        Errors.Add($"{Column.Title} is invalid");
                    }
                }
            }
        }

        #endregion Expands
    }
}