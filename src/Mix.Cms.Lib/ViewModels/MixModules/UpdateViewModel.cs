using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixModules
{
    //Use for update module info only => don't need to load data
    public class UpdateViewModel : ViewModelBase<MixCmsContext, MixModule, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("setAttributeId")]
        public int? SetAttributeId { get; set; }

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

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public MixModuleType Type { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        #endregion Models

        #region Views

        #region Attributes

        [JsonProperty("attributeSet")]
        public MixAttributeSets.UpdateViewModel AttributeSet { get; set; }

        #endregion Attributes

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("imageUrl")]
        public string ImageUrl {
            get {
                if (!string.IsNullOrEmpty(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl {
            get {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Thumbnail
                });
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
        public string TemplateFolderType {
            get {
                return MixEnums.EnumTemplateFolder.Modules.ToString();
            }
        }

        [JsonProperty("view")]
        public MixTemplates.UpdateViewModel View { get; set; }

        [JsonIgnore]
        public int ActivedTheme {
            get {
                return MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, Specificulture);
            }
        }

        [JsonIgnore]
        public string ThemeFolderType { get { return MixEnums.EnumTemplateFolder.Modules.ToString(); } }

        [JsonProperty("templateFolder")]
        public string TemplateFolder {
            get {
                return CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture)
                    , ThemeFolderType
                }
            );
            }
        }

        #endregion Template

        #region Form

        [JsonProperty("forms")]
        public List<MixTemplates.UpdateViewModel> Forms { get; set; }// Post Forms

        [JsonIgnore]
        public string FormFolderType {
            get {
                return MixEnums.EnumTemplateFolder.Forms.ToString();
            }
        }

        [JsonProperty("formView")]
        public MixTemplates.UpdateViewModel FormView { get; set; }

        [JsonProperty("formFolder")]
        public string FormFolder {
            get {
                return CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture)
                    , MixEnums.EnumTemplateFolder.Forms.ToString()
                }
            );
            }
        }

        #endregion Form

        #region Edm

        [JsonProperty("edms")]
        public List<MixTemplates.UpdateViewModel> Edms { get; set; }// Post Edms

        [JsonIgnore]
        public string EdmFolderType {
            get {
                return MixEnums.EnumTemplateFolder.Edms.ToString();
            }
        }

        [JsonProperty("edmView")]
        public MixTemplates.UpdateViewModel EdmView { get; set; }

        [JsonProperty("edmFolder")]
        public string EdmFolder {
            get {
                return CommonHelper.GetFullPath(new string[]
                {
                    MixConstants.Folder.TemplatesFolder
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeName, Specificulture)
                    , MixEnums.EnumTemplateFolder.Edms.ToString()
                }
            );
            }
        }

        #endregion Edm

        //Parent Post Id
        [JsonProperty("postId")]
        public string PostId { get; set; }

        //Parent Category Id
        [JsonProperty("pageId")]
        public int PageId { get; set; }

        public List<MixUrlAliases.UpdateViewModel> UrlAliases { get; set; }

        [JsonProperty("attributes")]
        public List<MixAttributeFields.UpdateViewModel> Attributes { get; set; }

        [JsonProperty("attributeData")]
        public MixRelatedAttributeDatas.UpdateViewModel AttributeData { get; set; }

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
            Template = View != null ? string.Format(@"{0}/{1}{2}", View.FolderType, View.FileName, View.Extension) : Template;
            FormTemplate = FormView != null ? string.Format(@"{0}/{1}{2}", FormView.FolderType, FormView.FileName, FormView.Extension) : FormTemplate;
            EdmTemplate = EdmView != null ? string.Format(@"{0}/{1}{2}", EdmView.FolderType, EdmView.FileName, EdmView.Extension) : EdmTemplate;

            var arrField = Columns != null ? JArray.Parse(
                Newtonsoft.Json.JsonConvert.SerializeObject(Columns.OrderBy(c => c.Priority).Where(
                    c => !string.IsNullOrEmpty(c.Name)))) : new JArray();
            Fields = arrField.ToString(Newtonsoft.Json.Formatting.None);
            if (!string.IsNullOrEmpty(Image) && Image[0] == '/') { Image = Image.Substring(1); }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Cultures = MixModules.Helper.LoadCultures(Id, Specificulture, _context, _transaction);
            Cultures.ForEach(c => c.IsSupported = _context.MixModule.Any(m => m.Id == Id && m.Specificulture == c.Specificulture));
            Columns = new List<ModuleFieldViewModel>();
            JArray arrField = !string.IsNullOrEmpty(Fields) ? JArray.Parse(Fields) : new JArray();
            foreach (var field in arrField)
            {
                ModuleFieldViewModel thisField = new ModuleFieldViewModel()
                {
                    Name = CommonHelper.ParseJsonPropertyName(field["name"].ToString()),
                    Title = field["title"]?.ToString(),
                    Options = field["options"] != null ? field["options"].Value<JArray>() : new JArray(),
                    Priority = field["priority"] != null ? field["priority"].Value<int>() : 0,
                    DataType = (MixDataType)(int)field["dataType"],
                    Width = field["width"] != null ? field["width"].Value<int>() : 3,
                    IsUnique = field["isUnique"] != null ? field["isUnique"].Value<bool>() : true,
                    IsRequired = field["isRequired"] != null ? field["isRequired"].Value<bool>() : true,
                    IsDisplay = field["isDisplay"] != null ? field["isDisplay"].Value<bool>() : true,
                    IsSelect = field["isSelect"] != null ? field["isSelect"].Value<bool>() : false,
                    IsGroupBy = field["isGroupBy"] != null ? field["isGroupBy"].Value<bool>() : false,
                };
                Columns.Add(thisField);
            }
            // Load Attributes
            LoadAttributes(_context, _transaction);

            this.Templates = this.Templates ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.TemplateFolderType).Data;
            this.View = MixTemplates.UpdateViewModel.GetTemplateByPath(Template, Specificulture, MixEnums.EnumTemplateFolder.Modules, _context, _transaction);
            this.Template = CommonHelper.GetFullPath(new string[]
               {
                    this.View?.FileFolder
                    , this.View?.FileName
               });

            this.Forms = this.Forms ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.FormFolderType).Data;
            this.FormView = MixTemplates.UpdateViewModel.GetTemplateByPath(FormTemplate, Specificulture, MixEnums.EnumTemplateFolder.Forms, _context, _transaction);
            this.FormTemplate = CommonHelper.GetFullPath(new string[]
               {
                    this.FormView?.FileFolder
                    , this.FormView?.FileName
               });

            this.Edms = this.Edms ?? MixTemplates.UpdateViewModel.Repository.GetModelListBy(
                t => t.Theme.Id == ActivedTheme && t.FolderType == this.EdmFolderType).Data;
            this.EdmView = MixTemplates.UpdateViewModel.GetTemplateByPath(EdmTemplate, Specificulture, MixEnums.EnumTemplateFolder.Edms, _context, _transaction);
            this.EdmTemplate = CommonHelper.GetFullPath(new string[]
               {
                    this.EdmView?.FileFolder
                    , this.EdmView?.FileName
               });
            if (SetAttributeId.HasValue)
            {
                AttributeSet = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(s => s.Id == SetAttributeId.Value).Data;
            }
            else
            {
                AttributeSet = new MixAttributeSets.UpdateViewModel();
            }
        }

        #region Async

        public override Task<RepositoryResponse<MixModule>> RemoveModelAsync(bool isRemoveRelatedModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            return base.RemoveModelAsync(isRemoveRelatedModels, _context, _transaction);
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixModule parent, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };

            var saveViewResult = await View.SaveModelAsync(true, _context, _transaction);
            ViewModelHelper.HandleResult(saveViewResult, ref result);

            if (result.IsSucceed && !string.IsNullOrEmpty(FormView.Content))
            {
                var saveResult = await FormView.SaveModelAsync(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            if (result.IsSucceed && !string.IsNullOrEmpty(EdmView.Content))
            {
                var saveResult = await EdmView.SaveModelAsync(true, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            if (result.IsSucceed)
            {
                // Save Attributes
                var saveResult = await SaveAttributeAsync(parent.Id, _context, _transaction);
                ViewModelHelper.HandleResult(saveResult, ref result);
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveAttributeAsync(int id, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            var getAttrs = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(m => m.Name == MixConstants.AttributeSetName.ADDITIONAL_FIELD_MODULE, context, transaction);
            if (getAttrs.IsSucceed)
            {
                Attributes = getAttrs.Data.Fields;
                AttributeData.AttributeSetId = getAttrs.Data.Id;
                AttributeData.AttributeSetName = getAttrs.Data.Name;
                AttributeData.Data.AttributeSetId = getAttrs.Data.Id;
                AttributeData.Data.AttributeSetName = getAttrs.Data.Name;
                AttributeData.ParentId = id.ToString();
                AttributeData.ParentType = (int)MixEnums.MixAttributeSetDataType.Module;
                var saveData = await AttributeData.Data.SaveModelAsync(true, context, transaction);
                ViewModelHelper.HandleResult(saveData, ref result);
                if (result.IsSucceed)
                {
                    AttributeData.Id = saveData.Data.Id;
                    var saveRelated = await AttributeData.SaveModelAsync(true, context, transaction);
                    ViewModelHelper.HandleResult(saveRelated, ref result);
                }
            }
            return result;
        }

        //public override List<Task> GenerateRelatedData(MixCmsContext context, IDbContextTransaction transaction)
        //{
        //    var tasks = new List<Task>();
        //    tasks.Add(Task.Run(() =>
        //    {
        //        AttributeData.Data.RemoveCache(AttributeData.Data.Model, context, transaction);
        //    }));
        //    foreach (var item in AttributeData.Data.Values)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            item.RemoveCache(item.Model, context, transaction);
        //        }));
        //    }
        //    // Remove parent Pages
        //    var relatedPages = context.MixPageModule.Include(m => m.MixPage).Where(d => d.Specificulture == Specificulture && (d.ModuleId == Id))
        //        .AsEnumerable();
        //    foreach (var item in relatedPages)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            MixPageModules.ReadMvcViewModel.Repository.RemoveCache(item, context, transaction);
        //        }));

        //        tasks.Add(Task.Run(() =>
        //        {
        //            MixPages.ReadViewModel.Repository.RemoveCache(item.MixPage, context, transaction);
        //        }));
        //    }

        //    return tasks;
        //}

        #endregion Async

        #endregion Overrides

        #region Expand

        private void LoadAttributes(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            LoadAttributeData(_context, _transaction);
            LoadAttributeFields(_context, _transaction);
            foreach (var field in Attributes.OrderBy(f => f.Priority))
            {
                var val = AttributeData.Data.Values.FirstOrDefault(v => v.AttributeFieldId == field.Id);
                if (val == null)
                {
                    val = new MixAttributeSetValues.UpdateViewModel(
                        new MixAttributeSetValue() { AttributeFieldId = field.Id }
                        , _context, _transaction);
                    val.DataType = field.DataType;
                    val.AttributeFieldName = field.Name;
                    val.Priority = field.Priority;
                    AttributeData.Data.Values.Add(val);
                }
                val.Priority = field.Priority;
                val.Field = field;
            }
        }

        private void LoadAttributeFields(MixCmsContext context, IDbContextTransaction transaction)
        {
            if (string.IsNullOrEmpty(AttributeData.Id))
            {
                var getAttrs = MixAttributeSets.UpdateViewModel.Repository.GetSingleModel(m => m.Name == MixConstants.AttributeSetName.ADDITIONAL_FIELD_MODULE, context, transaction);
                if (getAttrs.IsSucceed)
                {
                    Attributes = getAttrs.Data.Fields;
                }
            }
            else
            {
                Attributes = new List<MixAttributeFields.UpdateViewModel>();
                foreach (var item in AttributeData.Data.Values)
                {
                    if (item.Field != null)
                    {
                        Attributes.Add(item.Field);
                    }
                }
            }
        }

        private void LoadAttributeData(MixCmsContext context, IDbContextTransaction transaction)
        {
            AttributeData = MixRelatedAttributeDatas.UpdateViewModel.Repository.GetFirstModel(
                    a => a.ParentId == Id.ToString() && a.Specificulture == Specificulture && a.ParentType == (int)MixEnums.MixAttributeSetDataType.Module
                        , context, transaction).Data;
            if (AttributeData == null)
            {
                AttributeData = new MixRelatedAttributeDatas.UpdateViewModel(
                    new MixRelatedAttributeData()
                    {
                        Specificulture = Specificulture,
                        ParentType = (int)MixEnums.MixAttributeSetDataType.Module,
                        ParentId = Id.ToString()
                    }
                    );
                AttributeData.Data = new MixAttributeSetDatas.UpdateViewModel(
                new MixAttributeSetData()
                {
                    Specificulture = Specificulture
                }
                );
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

                case MixModuleType.SubPage:
                    getDataResult = MixModuleDatas.ReadViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       && (m.PageId == pageId)
                       , "Priority", 0, pageSize, pageIndex
                       , _context, _transaction);
                    break;

                case MixModuleType.SubPost:
                    getDataResult = MixModuleDatas.ReadViewModel.Repository
                       .GetModelListBy(m => m.ModuleId == Id && m.Specificulture == Specificulture
                       && (m.PostId == postId)
                       , "Priority", 0, pageSize, pageIndex
                       , _context, _transaction);
                    break;

                default:
                    break;
            }

            if (getDataResult.IsSucceed)
            {
                getDataResult.Data.JsonItems = new List<JObject>();
                getDataResult.Data.Items.ForEach(d => getDataResult.Data.JsonItems.Add(d.JItem));
                Data = getDataResult.Data;
            }
        }

        #endregion Expand
    }
}