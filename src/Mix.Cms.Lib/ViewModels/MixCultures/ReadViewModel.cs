using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.ViewModels.MixCultures
{
    public class ReadViewModel
      : ViewModelBase<MixCmsContext, MixCulture, ReadViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("lcid")]
        public string Lcid { get; set; }

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

        [JsonProperty("configurations")]
        public List<MixConfigurations.ReadMvcViewModel> Configurations { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public ReadViewModel() : base()
        {
        }

        public ReadViewModel(MixCulture model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
    }
}