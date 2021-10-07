using Mix.Database.Entities.Cms;
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

        public MixThemeViewModel(MixTheme entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
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

        #endregion
    }
}
