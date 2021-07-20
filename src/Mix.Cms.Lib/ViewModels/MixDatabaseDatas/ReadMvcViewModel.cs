using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDatas
{
    public class ReadMvcViewModel
      : ViewModelBase<MixCmsContext, MixDatabaseData, ReadMvcViewModel>
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

        [JsonProperty("values")]
        public List<MixDatabaseDataValues.ReadViewModel> Values { get; set; }

        [JsonProperty("obj")]
        public JObject Obj { get; set; }

        [JsonProperty("previewUrl")]
        public string PreviewUrl
        {
            get => !string.IsNullOrEmpty(Id) && HasValue("seo_url")
                    ? $"{MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain)}/data/{Specificulture}/{MixDatabaseName}/{Property<string>("seo_url")}"
                    : null;
        }

        [JsonProperty("detailApiUrl")]
        public string DetailApiUrl
        {
            get => !string.IsNullOrEmpty(Id)
                    ? $"{MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain)}/api/v1/rest/{Specificulture}/attribute-set-data/mvc/{Id}"
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

        public ReadMvcViewModel(MixDatabaseData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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

            Obj.LoadAllReferenceData(Id, MixDatabaseId, Specificulture, null, context, transaction);

            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }
        }

        public bool HasValue(string fieldName)
        {
            return Obj != null ? Obj.Value<string>(fieldName) != null : false;
        }

        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(Obj, fieldName);
        }

        #endregion Overrides
    }
}