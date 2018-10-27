using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixSystem;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixLanguages
{
    public class UpdateViewModel : ViewModelBase<MixCmsContext, MixLanguage, UpdateViewModel>
    {
        #region Properties

        #region Models

        [Required]
        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [Required]
        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }


        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }
        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain", Specificulture) ?? "/"; } }

        [JsonProperty("property")]
        public DataValueViewModel Property { get; set; }
        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixLanguage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        
        public override MixLanguage ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Value = Property.Value ?? Value;
            return base.ParseModel(_context, _transaction);
        }
        public override UpdateViewModel ParseView(bool isExpand = true, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Property = new DataValueViewModel() { DataType = DataType, Value = Value, Name = Keyword };
            return base.ParseView(isExpand, _context, _transaction);
        }
        #region Async
        public override Task<UpdateViewModel> ParseViewAsync(bool isExpand = true, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Property = new DataValueViewModel() { DataType = DataType, Value = Value, Name = Keyword };
            return base.ParseViewAsync(isExpand, _context, _transaction);
        }
        public override Task<bool> ExpandViewAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            this.Cultures.ForEach(c => c.IsSupported = true);
            IsClone = true;

            return base.ExpandViewAsync(_context, _transaction);
        }
        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            foreach (var culture in Cultures.Where(c => c.Specificulture != Specificulture))
            {
                var lang = _context.MixLanguage.First(c => c.Keyword == Keyword && c.Specificulture == culture.Specificulture);
                if (lang != null)
                {
                    _context.MixLanguage.Remove(lang);
                }
            }
            return new RepositoryResponse<bool>()
            {
                IsSucceed = (await _context.SaveChangesAsync()) > 0
            };
        }
        public override async Task<RepositoryResponse<UpdateViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.SaveModelAsync(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                MixService.LoadFromDatabase();
                MixService.Save();
            }
            return result;
        }

        public override async Task<RepositoryResponse<MixLanguage>> RemoveModelAsync(bool isRemoveRelatedModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = await base.RemoveModelAsync(isRemoveRelatedModels, _context, _transaction);
            if (result.IsSucceed)
            {
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.Save();
                }
            }
            return result;
        }

        #endregion
        
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            this.Cultures.ForEach(c => c.IsSupported = true);
            IsClone = true;
        }

        public override RepositoryResponse<bool> RemoveRelatedModels(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            foreach (var culture in Cultures.Where(c => c.Specificulture != Specificulture))
            {
                var lang = _context.MixLanguage.First(c => c.Keyword == Keyword && c.Specificulture == culture.Specificulture);
                if (lang != null)
                {
                    _context.MixLanguage.Remove(lang);
                }
            }
            return new RepositoryResponse<bool>()
            {
                IsSucceed = _context.SaveChanges() > 0
            };
        }



        #endregion Overrides

        #region Expand

        List<SupportedCulture> LoadCultures(string initCulture = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCultures = SystemCultureViewModel.Repository.GetModelList(_context, _transaction);
            var result = new List<SupportedCulture>();
            if (getCultures.IsSucceed)
            {
                foreach (var culture in getCultures.Data)
                {
                    result.Add(
                        new SupportedCulture()
                        {
                            Icon = culture.Icon,
                            Specificulture = culture.Specificulture,
                            Alias = culture.Alias,
                            FullName = culture.FullName,
                            Description = culture.FullName,
                            Id = culture.Id,
                            Lcid = culture.Lcid,
                            IsSupported = culture.Specificulture == initCulture || _context.MixLanguage.Any(p => p.Keyword == Keyword && p.Specificulture == culture.Specificulture)
                        });

                }
            }
            return result;
        }
        #endregion
    }
}
