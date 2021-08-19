using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixDatabaseColumns;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixModules
{
    //Use for update module info only => don't need to load data
    public class UpdateViewModel : ViewModelBase<MixCmsContext, MixModule, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

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

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("editorValue")]
        public string EditorValue { get; set; }

        [JsonProperty("editorType")]
        public MixEditorType? EditorType { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public MixModuleType Type { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isClone")]
        public bool IsClone { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return $"{Domain}/{Image}";
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl
        {
            get
            {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return $"{Domain}/{Thumbnail}";
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        [JsonProperty("data")]
        public PaginationModel<MixModuleDatas.ReadViewModel> Data { get; set; } = new PaginationModel<MixModuleDatas.ReadViewModel>();

        [JsonProperty("columns")]
        public List<ModuleFieldViewModel> Columns { get; set; }

        #region Template

        [JsonProperty("templates")]
        public List<MixTemplates.UpdateViewModel> Templates { get; set; }// Post Templates

        [JsonIgnore]
        public string TemplateFolderType
        {
            get
            {
                return MixTemplateFolders.Modules;
            }
        }

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonIgnore]
        public int ActivedTheme
        {
            get
            {
                return MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public string ThemeFolderType { get { return MixTemplateFolders.Modules.ToString(); } }

        [JsonProperty("templateFolder")]
        public string TemplateFolder
        {
            get
            {
                return $"{MixFolders.TemplatesFolder}/" +
                    $"{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeName, Specificulture)}/" +
                    $"{ThemeFolderType}";
            }
        }

        #endregion Template

        //Parent Post Id
        [JsonProperty("postId")]
        public string PostId { get; set; }

        //Parent Category Id
        [JsonProperty("pageId")]
        public int PageId { get; set; }

        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("sysCategories")]
        public List<MixDatabaseDataAssociations.UpdateViewModel> SysCategories { get; set; }

        [JsonProperty("sysTags")]
        public List<MixDatabaseDataAssociations.UpdateViewModel> SysTags { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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
            Template = View != null ? $"{View.FolderType}/{View.FileName}{View.Extension}" : Template;

            var arrField = Columns != null ? JArray.Parse(
                Newtonsoft.Json.JsonConvert.SerializeObject(Columns.OrderBy(c => c.Priority).Where(
                    c => !string.IsNullOrEmpty(c.Name)))) : new JArray();
            Fields = arrField.ToString(Newtonsoft.Json.Formatting.None);
            if (!string.IsNullOrEmpty(Image) && Image[0] == '/') { Image = Image.Substring(1); }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            EditorValue ??= Description;
            EditorType ??= MixEditorType.Html;
            Cultures = MixModules.Helper.LoadCultures(Id, Specificulture, _context, _transaction);
            Cultures.ForEach(c => c.IsSupported = _context.MixModule.Any(m => m.Id == Id && m.Specificulture == c.Specificulture));
            Columns = new List<ModuleFieldViewModel>();
            JArray arrField = !string.IsNullOrEmpty(Fields) ? JArray.Parse(Fields) : new JArray();
            foreach (var field in arrField)
            {
                var col = field.ToObject<ModuleFieldViewModel>();
                Columns.Add(col);
            }

            this.Templates = MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.TemplateFolderType, _context, _transaction).Data;
            var templateName = Template?.Substring(Template.LastIndexOf('/') + 1) ?? MixConstants.DefaultTemplate.Module;
            this.View = Templates.FirstOrDefault(t => !string.IsNullOrEmpty(templateName) && templateName.Equals($"{t.FileName}{t.Extension}"));
            this.View ??= Templates.FirstOrDefault();
            this.Template = $"{View?.FileFolder}/{View?.FileName}{View?.Extension}";
        }

        #region Async

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string parentId = Id.ToString();
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var removeAdditionalData = await MixDatabaseDataAssociations.UpdateViewModel.Repository.GetModelListByAsync(
                    m => m.ParentId == parentId
                        && m.MixDatabaseName == MixDatabaseNames.ADDITIONAL_COLUMN_MODULE
                        && m.ParentType == MixDatabaseParentType.Module
                        && m.Specificulture == Specificulture,
                    _context, _transaction
                        );
            foreach (var item in removeAdditionalData.Data)
            {
                var temp = await item.Data.RemoveModelAsync(true, _context, _transaction);
                ViewModelHelper.HandleResult(temp, ref result);
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixModule parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };

            if (View.Id == 0)
            {
                var saveViewResult = await View.SaveModelAsync(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveViewResult, ref result);
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> CloneSubModelsAsync(MixModule parent, List<SupportedCulture> cloneCultures, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string parentId = Id.ToString();
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var getAdditionalData = await MixDatabaseDataAssociations.FormViewModel.Repository.GetFirstModelAsync(
                    m => m.ParentId == parentId && m.ParentType == MixDatabaseParentType.Module && m.Specificulture == Specificulture,
                    _context, _transaction);
            if (getAdditionalData.IsSucceed)
            {
                //getAdditionalData.Data.Cultures = Cultures;
                //var m = getAdditionalData.Data.AttributeData.ParseModel(_context, _transaction);
                //var cloneValue = await getAdditionalData.Data.AttributeData.CloneAsync(m, Cultures, _context, _transaction);

                getAdditionalData.Data.Cultures = Cultures;
                var model = getAdditionalData.Data.ParseModel(_context, _transaction);
                var cloneData = await getAdditionalData.Data.CloneAsync(model, Cultures, _context, _transaction);
                ViewModelHelper.HandleResult(cloneData, ref result);
            }
            return result;
        }

        #endregion Async

        #endregion Overrides

        #region Expand

        public static async Task<RepositoryResponse<JObject>> SaveByModuleName(string culture, string createdBy, string name, string formName, JObject obj
       , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var getModule = await Repository.GetSingleModelAsync(m => m.Specificulture == culture && m.Name == name, context, transaction);
                string dataId = obj["id"]?.Value<string>();
                if (getModule.IsSucceed)
                {
                    // Get Attribute set
                    var getAttrSet = await Lib.ViewModels.MixDatabases.ReadViewModel.Repository.GetSingleModelAsync(m => m.Name == formName, context, transaction);
                    if (getAttrSet.IsSucceed)
                    {
                        // Save attr data + navigation
                        MixDatabaseDatas.UpdateViewModel data = new MixDatabaseDatas.UpdateViewModel()
                        {
                            Id = dataId,
                            CreatedBy = createdBy,
                            MixDatabaseId = getAttrSet.Data.Id,
                            MixDatabaseName = getAttrSet.Data.Name,
                            Specificulture = culture,
                            Data = obj
                        };

                        // Create navigation module - attr data
                        var getNavigation = await MixDatabaseDataAssociations.ReadViewModel.Repository.GetSingleModelAsync(
                            m => m.ParentId == getModule.Data.Id.ToString() && m.ParentType == MixDatabaseParentType.Module && m.Specificulture == culture
                            , context, transaction);
                        if (!getNavigation.IsSucceed)
                        {
                            data.RelatedData.Add(new MixDatabaseDataAssociations.UpdateViewModel()
                            {
                                ParentId = getModule.Data.Id.ToString(),
                                Specificulture = culture,
                                ParentType = MixDatabaseParentType.Module
                            });
                        }
                        var portalResult = await data.SaveModelAsync(true, context, transaction);
                        UnitOfWorkHelper<MixCmsContext>.HandleTransaction(portalResult.IsSucceed, isRoot, transaction);

                        return new RepositoryResponse<JObject>()
                        {
                            IsSucceed = portalResult.IsSucceed,
                            Data = portalResult.Data?.Data,
                            Exception = portalResult.Exception,
                            Errors = portalResult.Errors
                        };
                    }
                    else
                    {
                        return new RepositoryResponse<JObject>()
                        {
                            IsSucceed = false,
                            Status = (int)MixResponseStatus.BadRequest
                        };
                    }
                }
                else
                {
                    return new RepositoryResponse<JObject>()
                    {
                        IsSucceed = false,
                        Status = (int)MixResponseStatus.BadRequest
                    };
                }
            }
            catch (Exception ex)
            {
                return (UnitOfWorkHelper<MixCmsContext>.HandleException<JObject>(ex, isRoot, transaction));
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

        public void LoadData(int? postId = null, int? productId = null, int? pageId = null
            , int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<PaginationModel<MixModuleDatas.ReadViewModel>> getDataResult = new RepositoryResponse<PaginationModel<MixModuleDatas.ReadViewModel>>();

            switch (Type)
            {
                case MixModuleType.Content:
                    getDataResult = MixModuleDatas.ReadViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       , "Priority", 0, pageSize, pageIndex
                       , _context, _transaction);
                    break;

                default:
                    break;
            }

            if (getDataResult.IsSucceed)
            {
                //getDataResult.Data.JsonItems = new List<JObject>();
                //getDataResult.Data.Items.ForEach(d => getDataResult.Data.JsonItems.Add(d.JItem));
                Data = getDataResult.Data;
            }
        }

        #endregion Expand
    }
}