using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Abstracts.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.ViewModels.Cms;
using System.Collections.Generic;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportPageViewModel: MixPageViewModelBase<ImportPageViewModel>
    {
        #region Properties
        public List<MixUrlAliasViewModel> UrlAliases { get; set; }

        public bool IsExportData { get; set; }

        public string ThemeName { get; set; } = "default";

        public ImportMixDataAssociationViewModel RelatedData { get; set; }
        #endregion

        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            GetAdditionalData(Id.ToString(), MixDatabaseParentType.Page, _context, _transaction);
        }
        #endregion

        #region Expands

        public List<MixUrlAliasViewModel> GetAliases(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixUrlAliasViewModel.Repository.GetModelListBy(p => p.Specificulture == Specificulture
                        && p.SourceId == Id.ToString() && p.Type == (int)MixUrlAliasType.Page, context, transaction);
            if (result.IsSucceed)
            {
                return result.Data;
            }
            else
            {
                return new List<MixUrlAliasViewModel>();
            }
        }

        public List<MixPageModuleViewModel> GetModuleNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            return MixPageModuleViewModel.Repository.GetModelListBy(
                module => module.Specificulture == Specificulture && module.PageId == Id,
                context, transaction).Data;
        }

        public List<MixPagePostViewModel> GetPostNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            return MixPagePostViewModel.Repository.GetModelListBy(
                m => m.Specificulture == Specificulture && m.PageId == Id,
                context, transaction).Data;
        }

        private void GetAdditionalData(string id, MixDatabaseParentType type, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getRelatedData = ImportMixDataAssociationViewModel.Repository.GetFirstModel(
                        m => m.Specificulture == Specificulture && m.ParentType == type
                            && m.ParentId == id, context, transaction);
            if (getRelatedData.IsSucceed)
            {
                RelatedData = (getRelatedData.Data);
            }
        }

        #endregion Expands
    }
}
