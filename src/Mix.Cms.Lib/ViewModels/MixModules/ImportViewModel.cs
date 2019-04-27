using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixSystem;
using Mix.Common.Helper;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixModules
{
    //Use for update module info only => don't need to load data
    public class ImportViewModel : ViewModelBase<MixCmsContext, MixModule, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("formTemplate")]
        public string FormTemplate { get; set; }

        [JsonProperty("edmTemplate")]
        public string EdmTemplate { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public MixModuleType Type { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("data")]
        public PaginationModel<MixModuleDatas.ReadViewModel> Data { get; set; } = new PaginationModel<MixModuleDatas.ReadViewModel>();

        //Parent Article Id
        [JsonProperty("articleId")]
        public string ArticleId { get; set; }

        //Parent Category Id
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides
        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid && Id == 0)
            {
                IsValid = !Repository.CheckIsExists(m => m.Name == Name && m.Specificulture == Specificulture
                , _context, _transaction);
                if (!IsValid)
                {
                    Errors.Add("Module Name Existed");
                }
            }
        }
        public override MixModule ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = ReadListItemViewModel.Repository.Max(m => m.Id, _context, _transaction).Data + 1;
                LastModified = DateTime.UtcNow;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }


        #region Async

        public override Task<RepositoryResponse<MixModule>> RemoveModelAsync(bool isRemoveRelatedModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            return base.RemoveModelAsync(isRemoveRelatedModels, _context, _transaction);
        }


        #endregion Async

        #endregion Overrides

        #region Expand

        public void LoadData(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<PaginationModel<MixModuleDatas.ReadViewModel>> getDataResult = new RepositoryResponse<PaginationModel<MixModuleDatas.ReadViewModel>>();
            getDataResult = MixModuleDatas.ReadViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       , "Priority", 0, null, null
                       , _context, _transaction);

        }

        #endregion Expand
    }
}
