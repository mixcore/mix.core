using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Heart.Models;

namespace Mix.Cms.Lib.ViewModels.MixDatabases
{
    public class UpdateViewModel
      : ViewModelBase<MixCmsContext, MixDatabase, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ReferenceId")]
        public int? ReferenceId { get; set; }

        [JsonProperty("type")]
        public MixDatabaseType Type { get; set; } = MixDatabaseType.Service;

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("formTemplate")]
        public string FormTemplate { get; set; }

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

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("columns")]
        public List<MixDatabaseColumns.UpdateViewModel> Columns { get; set; }

        //[JsonProperty("formView")]
        //public MixTemplates.UpdateViewModel FormView { get; set; }

        //[JsonProperty("edmView")]
        //public MixTemplates.UpdateViewModel EdmView { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixDatabase model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id > 0)
            {
                Columns = MixDatabaseColumns.UpdateViewModel
                .Repository.GetModelListBy(a => a.MixDatabaseId == Id || a.MixDatabaseName == Name, _context, _transaction).Data?.OrderBy(a => a.Priority).ToList()
                ?? new List<MixDatabaseColumns.UpdateViewModel>();
            }
            else
            {
                Columns = new List<MixDatabaseColumns.UpdateViewModel>();
            }
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
                foreach (var item in Columns)
                {
                    if (result.IsSucceed)
                    {
                        item.MixDatabaseId = parent.Id;
                        item.MixDatabaseName = parent.Name;
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
                foreach (var item in Columns)
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