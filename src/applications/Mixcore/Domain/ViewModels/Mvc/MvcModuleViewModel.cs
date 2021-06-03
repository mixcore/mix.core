using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Mix.Lib;
using Mix.Lib.Abstracts.ViewModels;
using Mix.Shared.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using Mix.Lib.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Cms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mix.Shared.Services;

namespace Mixcore.Domain.ViewModels.Mvc
{
    public class MvcModuleViewModel : MixModuleViewModelBase<MvcModuleViewModel>
    {
        #region Properties

        public string Domain { get { return MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain); } }

        public string DetailsUrl { get; set; }

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

        public string ThumbnailUrl
        {
            get
            {
                if (Thumbnail != null && !Thumbnail.Contains("http", StringComparison.CurrentCulture) && Thumbnail[0] != '/')
                {
                    return $"{Domain}/{Thumbnail}";
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        public List<ModuleFieldModel> Columns
        {
            get { return Fields == null ? null : JsonConvert.DeserializeObject<List<ModuleFieldModel>>(Fields); }
            set { Fields = JsonConvert.SerializeObject(value); }
        }

        public MixTemplateViewModel View { get; set; }

        public MixTemplateViewModel FormView { get; set; }

        public MixTemplateViewModel EdmView { get; set; }

        public PaginationModel<MixModuleDataViewModel> Data { get; set; } = new PaginationModel<MixModuleDataViewModel>();

        public PaginationModel<MixPostViewModel> Posts { get; set; } = new PaginationModel<MixPostViewModel>();

        public string TemplatePath
        {
            get
            {
                return $"/{MixFolders.TemplatesFolder}/" +
                    $"{ConfigurationService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture) ?? "Default"}/" +
                    $"{Template}";
            }
        }

        public string FormTemplatePath
        {
            get
            {
                return $"/{MixFolders.TemplatesFolder}/" +
                   $"{ConfigurationService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture) ?? "Default"}/" +
                   $"{FormTemplate}";
            }
        }

        public string EdmTemplatePath
        {
            get
            {
                return $"/{MixFolders.TemplatesFolder}/" +
                   $"{ConfigurationService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture) ?? "Default"}/" +
                   $"{EdmTemplate}";
            }
        }

        public MixDatabaseDataViewModel AttributeData { get; set; }

        public int? PostId { get; set; }
        public int? PageId { get; set; }
        #endregion

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //Load Template + Style +  Scripts for views
            this.View = MixTemplateViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
            this.FormView = MixTemplateViewModel.GetTemplateByPath(FormTemplate, Specificulture, _context, _transaction).Data;
            this.EdmView = MixTemplateViewModel.GetTemplateByPath(EdmTemplate, Specificulture, _context, _transaction).Data;
            // call load data from controller for padding parameter (postId, productId, ...)
            AttributeData = MixDataHelper.LoadAdditionalData(Id.ToString(), Specificulture, MixDatabaseName.ADDITIONAL_FIELD_MODULE, _context, _transaction);
        }

        #endregion Overrides

        #region Expand

        
        public async Task LoadData(PagingRequestModel pagingRequest, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixModuleData, bool>> dataExp = null;
                Expression<Func<MixModulePost, bool>> postExp = null;
                switch (Type)
                {
                    case MixModuleType.Content:
                    case MixModuleType.Data:
                        await LoadDatasAsync(pagingRequest, context, transaction);
                        break;

                    case MixModuleType.ListPost:
                        await LoadPostsAsync(pagingRequest, context, transaction);
                        break;

                    default:
                        dataExp = m => m.ModuleId == Id && m.Specificulture == Specificulture;
                        postExp = n => n.ModuleId == Id && n.Specificulture == Specificulture;
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

        private async Task LoadDatasAsync(PagingRequestModel pagingRequest, MixCmsContext context, IDbContextTransaction transaction)
        {
            var getData = await MixModuleDataViewModel.Repository.GetModelListByAsync(
                    m => m.ModuleId == Id && m.Specificulture == Specificulture,
                        pagingRequest.OrderBy, pagingRequest.Direction, pagingRequest.PageSize, pagingRequest.PageIndex,
                        null, null,
                        context, transaction);
            Data = getData.Data;
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

        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(AttributeData?.Obj, fieldName);
        }

        #endregion Expand
    }
}
