using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace Mix.Cms.Lib.ViewModels.MixSystem
{
    public class SystemCultureViewModel
        : ViewModelBase<MixCmsContext, MixCulture, SystemCultureViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("lcid")]
        public string Lcid { get; set; }

        [Required]
        [JsonProperty("specificulture")]
        public new string Specificulture { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public SystemCultureViewModel() : base()
        {
        }

        public SystemCultureViewModel(MixCulture model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(SystemCultureViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var configs = await SystemConfigurationViewModel.Repository.GetModelListByAsync(c => c.Specificulture == view.Specificulture, _context, _transaction);
            if (configs.IsSucceed)
            {
                foreach (var item in configs.Data)
                {
                    var removeResult = await item.RemoveModelAsync(false, _context, _transaction);
                    result.IsSucceed = result.IsSucceed && removeResult.IsSucceed;
                    if (!result.IsSucceed)
                    {
                        result.Errors.AddRange(removeResult.Errors);
                        result.Exception = removeResult.Exception;
                        break;
                    }
                }
            }
            if (result.IsSucceed)
            {
                var languages = await SystemLanguageViewModel.Repository.GetModelListByAsync(c => c.Specificulture == view.Specificulture, _context, _transaction);
                if (languages.IsSucceed)
                {
                    foreach (var item in languages.Data)
                    {
                        var removeResult = await item.RemoveModelAsync(false, _context, _transaction);
                        result.IsSucceed = result.IsSucceed && removeResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Errors.AddRange(removeResult.Errors);
                            result.Exception = removeResult.Exception;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        #endregion Overrides
    }
}
