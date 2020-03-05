using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class ImportViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSet, ImportViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

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
        public int Status { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("fields")]
        public List<MixAttributeFields.UpdateViewModel> Fields { get; set; }

        [JsonIgnore]
        [JsonProperty("data")]
        public List<MixAttributeSetDatas.ImportViewModel> Data { get; set; }

        [JsonProperty("isExportData")]
        public bool IsExportData { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportViewModel() : base()
        {
        }

        public ImportViewModel(MixAttributeSet model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            //Fields = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(a => a.AttributeSetId == Id, _context, _transaction).Data?.OrderBy(a => a.Priority).ToList();
            //Data = MixAttributeSetDatas.UpdateViewModel.Repository.GetModelListBy(a => a.AttributeSetId == Id, _context, _transaction).Data?.OrderBy(a => a.Priority).ToList();
        }

        public override MixAttributeSet ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(s => s.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
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
            // Save Fields
            if (Fields != null)
            {
                result = await SaveFieldsAsync(parent, _context, _transaction);
            }
            //if (result.IsSucceed)
            //{
            //    // Save Data
            //    result = await SaveDataAsync(parent, _context, _transaction);
            //}

            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveDataAsync(MixAttributeSet parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (Data != null)
            {
                foreach (var item in Data)
                {
                    if (result.IsSucceed)
                    {
                        item.AttributeSetId = parent.Id;
                        item.AttributeSetName = parent.Name;
                        item.Fields = Fields;
                        item.CreatedDateTime = DateTime.UtcNow;
                        var saveResult = await item.SaveModelAsync(true, context, transaction);
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

        private async Task<RepositoryResponse<bool>> SaveFieldsAsync(MixAttributeSet parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Fields)
            {
                if (result.IsSucceed)
                {
                    item.AttributeSetId = parent.Id;
                    item.AttributeSetName = parent.Name;
                    var saveResult = await item.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveResult, ref result);
                }
                else
                {
                    break;
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
        //            MixAttributeSetDatas.ImportViewModel.Repository.RemoveCache(item, context, transaction);
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
    }
}