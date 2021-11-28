using Microsoft.AspNetCore.Http;
using Mix.Heart.Models;

namespace Mix.Lib.ViewModels
{
    public class MixThemeViewModel
        : SiteDataViewModelBase<MixCmsContext, MixTheme, int, MixThemeViewModel>
    {
        #region Properties
        public string SystemName { get; set; }
        public string PreviewUrl { get; set; }
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }

        public string AssetFolder
        {
            get
            {
                return $"{MixFolders.SiteContentAssetsFolder}/{SystemName}/assets";
            }
        }

        public string UploadsFolder
        {
            get
            {
                return $"{MixFolders.SiteContentAssetsFolder}/{SystemName}/uploads";
            }
        }

        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/{SystemName}";
            }
        }
        #endregion

        #region Contructors

        public MixThemeViewModel()
        {
        }

        public MixThemeViewModel(MixTheme entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null)
            : base(entity, cacheService, uowInfo)
        {
        }

        public MixThemeViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task<MixTheme> ParseEntity(MixCacheService cacheService = null)
        {
            if (string.IsNullOrEmpty(SystemName))
            {
                SystemName = SeoHelper.GetSEOString(DisplayName);
            }
            return base.ParseEntity(cacheService);
        }

        protected override Task SaveEntityRelationshipAsync(MixTheme parentEntity)
        {
            // Create default extra data
            MixDataContentViewModel extraData = new();

            return base.SaveEntityRelationshipAsync(parentEntity);
        }

        #endregion

        #region Expands
        #endregion
    }
}
