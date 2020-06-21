using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.OData.ViewModels.MixAttributeSets
{
    public class ODataUpdateViewModel
      : ODataViewModelBase<MixCmsContext, MixAttributeSet, ODataUpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
        [JsonProperty("cultures")]
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }

        [JsonProperty("ReferenceId")]
        public int? ReferenceId { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("formTemplate")]
        public string FormTemplate { get; set; }

        [JsonProperty("edmTemplate")]
        public string EdmTemplate { get; set; }

        [JsonProperty("edmSubject")]
        public string EdmSubject { get; set; }

        [JsonProperty("edmFrom")]
        public string EdmFrom { get; set; }

        [JsonProperty("edmAutoSend")]
        public bool? EdmAutoSend { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("fields")]
        public List<Lib.ViewModels.MixAttributeFields.UpdateViewModel> Fields { get; set; }

        [JsonProperty("removeAttributes")]
        public List<Lib.ViewModels.MixAttributeFields.DeleteViewModel> RemoveAttributes { get; set; } = new List<Lib.ViewModels.MixAttributeFields.DeleteViewModel>();

        [JsonProperty("formView")]
        public Lib.ViewModels.MixTemplates.UpdateViewModel FormView { get; set; }

        [JsonProperty("edmView")]
        public Lib.ViewModels.MixTemplates.UpdateViewModel EdmView { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ODataUpdateViewModel() : base()
        {
        }

        public ODataUpdateViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id > 0)
            {
                Fields = Lib.ViewModels.MixAttributeFields.UpdateViewModel
                .Repository.GetModelListBy(a => a.AttributeSetId == Id, _context, _transaction).Data?.OrderBy(a => a.Priority).ToList();
                FormView = Lib.ViewModels.MixTemplates.UpdateViewModel.GetTemplateByPath(FormTemplate, Specificulture, _context, _transaction).Data;
                EdmView = Lib.ViewModels.MixTemplates.UpdateViewModel.GetTemplateByPath(EdmTemplate, Specificulture, _context, _transaction).Data;
            }
            else
            {
                Fields = new List<Lib.ViewModels.MixAttributeFields.UpdateViewModel>();
            }
        }

        public override MixAttributeSet ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            FormTemplate = FormView != null ? string.Format(@"{0}/{1}{2}", FormView.FolderType, FormView.FileName, FormView.Extension) : FormTemplate;
            EdmTemplate = EdmView != null ? string.Format(@"{0}/{1}{2}", EdmView.FolderType, EdmView.FileName, EdmView.Extension) : EdmTemplate;
            return base.ParseModel(_context, _transaction);
        }

        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                if (_context.MixAttributeSet.Any(s => s.Name == Name && s.Id != Id))
                {
                    IsValid = false;
                    Errors.Add($"{Name} is Existed");
                }
            }
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSet parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Fields)
                {
                    if (result.IsSucceed)
                    {
                        item.AttributeSetId = parent.Id;
                        item.AttributeSetName = parent.Name;
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (result.IsSucceed)
            {
                foreach (var item in RemoveAttributes)
                {
                    if (result.IsSucceed)
                    {
                        var removeResult = await item.RemoveModelAsync(false, _context, _transaction);
                        ViewModelHelper.HandleResult(removeResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public override RepositoryResponse<bool> SaveSubModels(MixAttributeSet parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Fields)
                {
                    if (result.IsSucceed)
                    {
                        item.AttributeSetName = parent.Name;
                        item.AttributeSetId = parent.Id;
                        var saveResult = item.SaveModel(false, _context, _transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        //public override List<Task> GenerateRelatedData(MixCmsContext context, IDbContextTransaction transaction)
        //{
        //    var tasks = new List<Task>();
        //    var attrDatas = context.MixAttributeSetData.Where(m => m.AttributeSetId == Id);
        //    var attrFields = context.MixAttributeField.Where(m => m.AttributeSetId == Id);

        //    foreach (var item in attrDatas)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            MixAttributeSetDatas.UpdateViewModel.Repository.RemoveCache(item, context, transaction);
        //        }));
        //    }
        //    foreach (var item in attrFields)
        //    {
        //        tasks.Add(Task.Run(() =>
        //        {
        //            MixAttributeFields.UpdateViewModel.Repository.RemoveCache(item, context, transaction);
        //        }));
        //    }
        //    return tasks;
        //}

        #endregion Overrides

        #region Expand

        #endregion
    }
}