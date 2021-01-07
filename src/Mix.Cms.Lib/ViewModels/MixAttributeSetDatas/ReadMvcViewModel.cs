using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDataValues;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, ReadMvcViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("cultures")]
        public List<Domain.Core.Models.SupportedCulture> Cultures { get; set; }

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }
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
        public MixEnums.MixContentStatus Status { get; set; }
        #endregion Models

        #region Views
        [JsonProperty("values")]
        public List<MixAttributeSetValues.ReadViewModel> Values { get; set; }

        [JsonProperty("obj")]
        public JObject Obj { get; set; }

        [JsonProperty("detailsUrl")]
        public string DetailsUrl { 
            get => !string.IsNullOrEmpty(Id) && HasValue("seo_url") 
                    ? $"/data/{Specificulture}/{AttributeSetName}/{Property<string>("seo_url")}" 
                    : null; 
        }

        [JsonProperty("templatePath")]
        public string TemplatePath { get => $"{MixCmsHelper.GetTemplateFolder(Specificulture)}/{Property<string>("template_path")}"; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadMvcViewModel() : base()
        {
        }

        public ReadMvcViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                  _context, _transaction,
                  out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            
            if (Obj == null)
            {
                Obj = Helper.ParseData(Id, Specificulture, context, transaction);
            }
            
            Obj.LoadReferenceData(Id, AttributeSetId, Specificulture, context, transaction);

            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }
        }

        #endregion Overrides

        #region Expands
        public bool HasValue(string fieldName)
        {
            return Obj.Value<string>(fieldName) != null;
        }
        
        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(Obj, fieldName);
        }

        #endregion Expands
    }
}