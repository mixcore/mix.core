using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Mix.Lib.Abstracts.ViewModels;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixcore.Domain.ViewModels.Mvc
{
    public class MvcPageViewModel : MixPageViewModelBase<MvcPageViewModel>
    {
        #region Properties
        public string DetailsUrl { get => Id > 0 ? $"/{Specificulture}/page/{SeoName}" : null; }

        public string Domain { get { return MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain); } }

        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (!Image.Contains("http", StringComparison.CurrentCulture)) && Image[0] != '/')
                {
                    return $"{Domain}/{Image}";
                }
                else
                {
                    return Image;
                }
            }
        }

        public MixTemplateViewModel View { get; set; }

        public PaginationModel<MixPostViewModel> Posts { get; set; } = new PaginationModel<MixPostViewModel>();

        public List<MvcModuleViewModel> Modules { get; set; } = new List<MvcModuleViewModel>(); // Get All Module

        public string TemplatePath
        {
            get
            {
                return $"/{MixFolders.TemplatesFolder}/{MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture)}/{Template}";
            }
        }

        public MixDatabaseDataViewModel AttributeData { get; set; }

        public string BodyClass => CssClass;

        #endregion

        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.View = MixTemplateViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
            if (View != null)
            {
                GetSubModulesAsync(_context, _transaction).GetAwaiter().GetResult();
            }
            AttributeData = MixDataHelper.LoadAdditionalData(Id.ToString(), Specificulture, MixDatabaseName.ADDITIONAL_FIELD_PAGE, _context, _transaction);
        }
        #endregion

        #region Expands

        #region Sync

        public async Task LoadData(PagingRequestModel pagingRequest, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                foreach (var item in Modules)
                {
                    await item.LoadData(pagingRequest, _context: context, _transaction: transaction);
                }

                switch (Type)
                {
                    case MixPageType.ListPost:
                        await LoadPostsAsync(pagingRequest, context, transaction);
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<bool>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private async Task LoadPostsAsync(PagingRequestModel pagingRequest, MixCmsContext context, IDbContextTransaction transaction)
        {
            var query = MixAssociationHelper.GetAssociationModulePosts(Id, Specificulture, context);
            var getPosts = await MixPostViewModel.Repository.GetModelListByAsync(
                m => query.Any(p => p.Id == m.Id && p.Specificulture == m.Specificulture),
                pagingRequest.OrderBy, pagingRequest.Direction, pagingRequest.PageSize, pagingRequest.PageIndex,
                null, null,
                context, transaction);
            Posts = getPosts.Data;
        }


        private async Task GetSubModulesAsync(MixCmsContext _context, IDbContextTransaction _transaction = null)
        {
            var getModulesQuery = MixAssociationHelper.GetAssociationModules(Id, Specificulture, _context).OrderBy(m => m.Priority);
            var getModules = await MvcModuleViewModel.Repository.GetModelListByAsync(
                    m => getModulesQuery.Any(
                            association => association.Id == m.Id && association.Specificulture == m.Specificulture),
                            _context, _transaction);
            if (getModules.IsSucceed)
            {
                Modules = getModules.Data;
                StringBuilder scripts = new();
                StringBuilder styles = new();
                foreach (var nav in Modules.OrderBy(n => n.Priority).ToList())
                {
                    string script = $"<!-- Start script module {nav.Name} --> {nav.View?.Scripts} <!-- End script module {nav.Name} -->";
                    string style = $"<!-- Start style module {nav.Name} --> {nav.View?.Styles} <!-- End style module {nav.Name} -->";
                    scripts.Append(script);
                    styles.Append(style);
                }
                View.Scripts += scripts.ToString();
                View.Styles += styles.ToString();
            }
        }

        #endregion Sync

        public MvcModuleViewModel GetModule(string name)
        {
            return Modules.FirstOrDefault(m => m.Name == name);
        }

        public bool HasValue(string fieldName)
        {
            return AttributeData != null && AttributeData.Obj.GetValue(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(AttributeData?.Obj, fieldName);
        }

        #endregion Expands
    }
}
