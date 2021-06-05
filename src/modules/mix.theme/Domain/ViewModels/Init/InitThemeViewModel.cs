using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mix.Database.Entities.Cms.v2;
using Mix.Shared.Services;

namespace Mix.Theme.Domain.ViewModels.Init
{
    public class InitThemeViewModel
      : ViewModelBase<MixCmsContext, MixTheme, InitThemeViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public string Specificulture { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        public bool IsCreateDefault { get; set; }

        public bool IsActived { get; set; }

        public FileViewModel TemplateAsset { get; set; }

        public FileViewModel Asset { get; set; }

        public string AssetFolder
        {
            get
            {
                return $"{MixFolders.SiteContentAssetsFolder}/{Name}/assets";
            }
        }

        public string UploadsFolder
        {
            get
            {
                return $"{MixFolders.SiteContentAssetsFolder}/{Name}/uploads";
            }
        }

        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/{Name}";
            }
        }

        public List<InitTemplateViewModel> Templates { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public InitThemeViewModel()
            : base()
        {
        }

        public InitThemeViewModel(MixTheme model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixTheme ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Id = 1;
            Name = SeoHelper.GetSEOString(Title);
            CreatedDateTime = DateTime.UtcNow;
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Templates = InitTemplateViewModel.Repository.GetModelListBy(t => t.MixThemeId == Id,
                _context: _context, _transaction: _transaction).Data;
            TemplateAsset = new FileViewModel() { FileFolder = $"{MixFolders.ImportFolder}/{DateTime.UtcNow.ToShortDateString()}/{Name}" };
            Asset = new FileViewModel() { FileFolder = $"{MixFolders.WebRootPath}/{AssetFolder}" };
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixTheme parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<bool> result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (string.IsNullOrEmpty(TemplateAsset.Filename))
            {
                TemplateAsset = new FileViewModel()
                {
                    Filename = "default_blank",
                    Extension = MixFileExtensions.Zip,
                    FileFolder = MixFolders.ImportFolder
                };
            }

            result = await ImportThemeAsync(parent, _context, _transaction);

            // Actived Theme
            if (IsActived)
            {
                result = await ActivedThemeAsync(_context, _transaction);
            }

            return result;
        }

        private Task<RepositoryResponse<bool>> ActivedThemeAsync(MixCmsContext context, IDbContextTransaction transaction)
        {
            throw new NotImplementedException();
        }

        private Task<RepositoryResponse<bool>> ImportThemeAsync(MixTheme parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            throw new NotImplementedException();
        }


        #endregion Async

        #endregion Overrides
    }
}
