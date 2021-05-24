using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using MixLibViewModels = Mix.Cms.Lib.ViewModels;
namespace Mix.Rest.Api.Client.ViewModels
{
    public class DataViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseData, DataViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("cultures")]
        public List<SupportedCulture> Cultures { get; set; }

        [JsonProperty("mixDatabaseId")]
        public int MixDatabaseId { get; set; }

        [JsonProperty("mixDatabaseName")]
        public string MixDatabaseName { get; set; }

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

        [JsonProperty("detailsUrl")]
        public string DetailsUrl
        {
            get => !string.IsNullOrEmpty(Id) && HasValue("seo_url")
                    ? $"/data/{Specificulture}/{MixDatabaseName}/{Property<string>("seo_url")}"
                    : null;
        }

        [JsonProperty("obj")]
        public JObject Obj { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public MixDatabaseParentType ParentType { get; set; }

        [JsonProperty("relatedData")]
        public List<DataViewModel> RelatedData { get; set; } = new List<DataViewModel>();

        [JsonIgnore]
        public List<MixLibViewModels.MixDatabaseDataValues.UpdateViewModel> Values { get; set; }

        [JsonProperty("columns")]
        public List<MixLibViewModels.MixDatabaseColumns.UpdateViewModel> Columns { get; set; }

        [JsonIgnore]
        public List<DataViewModel> RefData { get; set; } = new List<DataViewModel>();

        #endregion Views

        #endregion Properties

        #region Contructors

        public DataViewModel() : base()
        {
        }

        public DataViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Columns ??= MixLibViewModels.MixDatabaseColumns.UpdateViewModel.Repository.GetModelListBy(f => f.MixDatabaseId == MixDatabaseId
           , _context, _transaction).Data;

            if (Obj == null)
            {
                Obj = MixLibViewModels.MixDatabaseDatas.Helper.ParseData(Id, Specificulture, _context, _transaction);
            }
            if (Columns.Any(c => c.DataType == MixDataType.Reference))
            {
                Obj.LoadAllReferenceData(Id, MixDatabaseId, Specificulture, 
                    Columns
                    .Where(c=>c.DataType == MixDataType.Reference)
                    .Select(c => new MixDatabaseColumn() 
                    { 
                        Name = c.Name,
                        ReferenceId = c.ReferenceId,
                        DataType = c.DataType
                    })
                    .ToList()
                        , _context, _transaction);
            }
        }

       
        #endregion Overrides

        #region Expands

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