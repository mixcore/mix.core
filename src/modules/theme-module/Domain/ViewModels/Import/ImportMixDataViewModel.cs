using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Mix.Lib.Extensions;

namespace Mix.Theme.Domain.ViewModels.Import
{
    public class ImportMixDataViewModel: ViewModelBase<MixCmsContext, MixDatabaseData, ImportMixDataViewModel>
    {
        #region Properties

        #region Models

        public string Id { get; set; }

        public string Specificulture { get; set; }

        public List<SupportedCulture> Cultures { get; set; }

        public int MixDatabaseId { get; set; }

        public string MixDatabaseName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        public JObject Obj { get; set; }

        public string ParentId { get; set; }

        public MixDatabaseParentType ParentType { get; set; }

        public List<ImportMixDataAssociationViewModel> RelatedData { get; set; } = new List<ImportMixDataAssociationViewModel>();

        public List<ImportMixDataValueViewModel> Values { get; set; }

        public List<ImportaMixDatabaseColumnViewModel> Columns { get; set; }

        [JsonIgnore]
        public List<ImportMixDataViewModel> RefData { get; set; } = new List<ImportMixDataViewModel>();

        #endregion Views

        #endregion Properties

        #region Contructors

        public ImportMixDataViewModel() : base()
        {
        }

        public ImportMixDataViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Obj == null)
            {
                Obj = MixDataHelper.ParseData(Id, Specificulture, _context, _transaction);
            }
        }

        public override MixDatabaseData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                CreatedDateTime = DateTime.UtcNow;
                Priority = Repository.Count(m => m.MixDatabaseName == MixDatabaseName && m.Specificulture == Specificulture, _context, _transaction).Data + 1;
            }

            if (string.IsNullOrEmpty(MixDatabaseName))
            {
                MixDatabaseName = _context.MixDatabase.First(m => m.Id == MixDatabaseId)?.Name;
            }
            if (MixDatabaseId == 0)
            {
                MixDatabaseId = _context.MixDatabase.First(m => m.Name == MixDatabaseName)?.Id ?? 0;
            }
            Values ??= ImportMixDataValueViewModel
                .Repository.GetModelListBy(a => a.DataId == Id && a.Specificulture == Specificulture
                , _context, _transaction)
                .Data.OrderBy(a => a.Priority).ToList();

            Columns ??= ImportaMixDatabaseColumnViewModel.Repository.GetModelListBy(
                    f => f.MixDatabaseId == MixDatabaseId,
                    _context, _transaction).Data;

            foreach (var field in Columns.OrderBy(f => f.Priority))
            {
                var val = Values.FirstOrDefault(v => v.MixDatabaseColumnId == field.Id);
                if (val == null)
                {
                    val = new ImportMixDataValueViewModel(
                        new MixDatabaseDataValue()
                        {
                            MixDatabaseColumnId = field.Id,
                            MixDatabaseColumnName = field.Name,
                        }
                        , _context, _transaction)
                    {
                        StringValue = field.DefaultValue,
                        Priority = field.Priority,
                        Column = field
                    };
                    Values.Add(val);
                }
                else
                {
                    val.LastModified = DateTime.UtcNow;
                }
                val.CreatedBy = CreatedBy;
                val.Status = Status;
                val.Priority = field.Priority;
                val.MixDatabaseName = MixDatabaseName;
                if (Obj[val.MixDatabaseColumnName] != null)
                {
                    if (val.Column.DataType == MixDataType.Reference)
                    {
                        var arr = Obj[val.MixDatabaseColumnName].Value<JArray>();
                        if (arr != null)
                        {
                            foreach (JObject objData in arr)
                            {
                                string id = objData["id"]?.Value<string>();
                                // if have id => update data, else add new
                                if (!string.IsNullOrEmpty(id))
                                {
                                    var getData = Repository.GetSingleModel(
                                        m => m.Id == id && m.Specificulture == Specificulture,
                                        _context, _transaction);
                                    if (getData.IsSucceed)
                                    {
                                        getData.Data.Obj = objData["obj"].Value<JObject>();
                                        RefData.Add(getData.Data);
                                    }
                                }
                                else
                                {
                                    RefData.Add(new ImportMixDataViewModel()
                                    {
                                        Specificulture = Specificulture,
                                        MixDatabaseId = field.ReferenceId.Value,
                                        Obj = objData["obj"].Value<JObject>()
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        val.ToModelValue(Obj[val.MixDatabaseColumnName], _context, _transaction);
                    }
                }
                else
                {
                    Obj.Add(val.Model.ToJProperty());
                }
            }

            return base.ParseModel(_context, _transaction); ;
        }

        #region Async

        public override async Task<RepositoryResponse<ImportMixDataViewModel>> SaveModelAsync(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = await base.SaveModelAsync(isSaveSubModels, context, transaction);
                if (result.IsSucceed && !string.IsNullOrEmpty(ParentId))
                {
                    var getNav = ImportMixDataAssociationViewModel.Repository.CheckIsExists(
                        m => m.DataId == Id && m.ParentId == ParentId && m.ParentType == ParentType && m.Specificulture == Specificulture
                        , context, transaction);
                    if (!getNav)
                    {
                        var nav = new ImportMixDataAssociationViewModel()
                        {
                            DataId = Id,
                            Specificulture = Specificulture,
                            MixDatabaseId = MixDatabaseId,
                            MixDatabaseName = MixDatabaseName,
                            ParentType = ParentType,
                            ParentId = ParentId,
                            Status = MixContentStatus.Published
                        };
                        var saveResult = await nav.SaveModelAsync(false, context, transaction);
                        if (!saveResult.IsSucceed)
                        {
                            result.IsSucceed = false;
                            result.Exception = saveResult.Exception;
                            result.Errors = saveResult.Errors;
                        }
                    }
                }

                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                if (result.IsSucceed)
                {
                    Obj = MixDataHelper.ParseData(Id, Specificulture, context, transaction);
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<ImportMixDataViewModel>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    transaction.Dispose();
                    context.Dispose();
                }
            }
        }

        public override RepositoryResponse<ImportMixDataViewModel> SaveModel(bool isSaveSubModels = false, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = base.SaveModel(isSaveSubModels, _context, _transaction);
            if (result.IsSucceed)
            {
                Obj = MixDataHelper.ParseData(Id, Specificulture, _context, _transaction);
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixDatabaseData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };

            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveValues = await SaveValues(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveValues, ref result);
            }
            // Save Ref Data
            if (result.IsSucceed)
            {
                RepositoryResponse<bool> saveRefData = await SaveRefDataAsync(parent, _context, _transaction);
                ViewModelHelper.HandleResult(saveRefData, ref result);
            }

            //// Save Related Data
            //if (result.IsSucceed)
            //{
            //    RepositoryResponse<bool> saveRelated = await SaveRelatedDataAsync(parent, _context, _transaction);
            //    ViewModelHelper.HandleResult(saveRelated, ref result);
            //}

            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveValues(MixDatabaseData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in Values)
            {
                if (result.IsSucceed)
                {
                    if (Columns.Any(f => f.Id == item.MixDatabaseColumnId))
                    {
                        item.DataId = parent.Id;
                        item.Specificulture = parent.Specificulture;
                        item.Priority = item.Column.Priority;
                        item.Status = MixContentStatus.Published;
                        var saveResult = await item.SaveModelAsync(false, context, transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                    else
                    {
                        var delResult = await item.RemoveModelAsync(false, context, transaction);
                        ViewModelHelper.HandleResult(delResult, ref result);
                    }
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> SaveRefDataAsync(MixDatabaseData parent, MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            foreach (var item in RefData)
            {
                if (result.IsSucceed)
                {
                    item.Specificulture = Specificulture;
                    item.ParentId = parent.Id;
                    item.ParentType = MixDatabaseParentType.Set;
                    item.Status = MixContentStatus.Published;
                    var saveRef = await item.SaveModelAsync(true, context, transaction);
                    if (saveRef.IsSucceed)
                    {
                        RelatedData.Add(new ImportMixDataAssociationViewModel()
                        {
                            DataId = saveRef.Data.Id,
                            ParentId = Id,
                            ParentType = MixDatabaseParentType.Set,
                            MixDatabaseId = saveRef.Data.MixDatabaseId,
                            MixDatabaseName = saveRef.Data.MixDatabaseName,
                            CreatedDateTime = DateTime.UtcNow,
                            Specificulture = Specificulture
                        });
                    }
                    ViewModelHelper.HandleResult(saveRef, ref result);
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        #endregion Async

        #endregion Overrides

        #region Expands

        public static async Task<RepositoryResponse<ImportMixDataViewModel>> SaveObjectAsync(JObject data, string mixDatabaseName)
        {
            var vm = new ImportMixDataViewModel()
            {
                Id = data["id"]?.Value<string>(),
                Specificulture = data["specificulture"]?.Value<string>(),
                MixDatabaseName = mixDatabaseName,
                Obj = data
            };
            return await vm.SaveModelAsync();
        }

        public bool HasValue(string fieldName)
        {
            return Obj != null && Obj.Value<string>(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            if (Obj != null)
            {
                return Obj.Value<T>(fieldName);
            }
            else
            {
                return default;
            }
        }

        #endregion Expands
    }
}
