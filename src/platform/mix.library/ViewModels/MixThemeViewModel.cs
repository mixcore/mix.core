using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Cms;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Shared.Constants;
using System;
using System.Threading.Tasks;

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

        protected override Task SaveEntityRelationshipAsync(MixTheme parentEntity)
        {
            // Create default extra data
            MixDataContentViewModel extraData = new();

            return base.SaveEntityRelationshipAsync(parentEntity);
        }

        #endregion

        #region Expands

        public async Task<bool> SaveThemeAsync(IFormFile file, UnitOfWorkInfo uow = null)
        {
            // Load default blank if created new without upload theme
            FileModel fileModel = LoadFile(file);

            MixFileService.Instance.SaveFile(fileModel);

            var result = await SaveAsync(uow);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        private FileModel LoadFile(IFormFile file)
        {
            if (Id == 0 && file == null)
            {
                return new()
                {
                    Filename = "_blank",
                    Extension = MixFileExtensions.Zip,
                    FileFolder = MixFolders.JsonDataFolder
                };
            }
            string importFolder = $"{MixFolders.ThemePackage}/{DateTime.UtcNow.ToString("dd-MM-yyyy")}/{SystemName}";
            return new(file, importFolder);
        }

        #endregion
    }
}
