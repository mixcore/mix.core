using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportMixDatabaseViewModel : ViewModelBase<MixCmsContext, MixDatabase, ImportMixDatabaseViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public int? ReferenceId { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FormTemplate { get; set; }

        public string EdmTemplate { get; set; }

        public string EdmSubject { get; set; }

        public string EdmFrom { get; set; }

        public bool? EdmAutoSend { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        public List<ImportaMixDatabaseColumnViewModel> Fields { get; set; }

        [JsonIgnore]
        public List<ImportMixDataViewModel> Data { get; set; }

        public bool IsExportData { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportMixDatabaseViewModel() : base()
        {
        }

        public ImportMixDatabaseViewModel(MixDatabase model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
        }

        public override MixDatabase ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (_context.MixDatabase.Any(s => s.Name == Name && s.Id != Id))
                {
                    IsValid = false;
                    Errors.Add($"{Name} is Existed");
                }
            }
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixDatabase parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Fields)
                {
                    if (result.IsSucceed)
                    {
                        item.MixDatabaseName = parent.Name;
                        item.MixDatabaseId = parent.Id;
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public override RepositoryResponse<bool> SaveSubModels(MixDatabase parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Fields)
                {
                    if (result.IsSucceed)
                    {
                        item.MixDatabaseName = parent.Name;
                        item.MixDatabaseId = parent.Id;
                        var saveResult = item.SaveModel(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        #endregion Overrides
    }
}
