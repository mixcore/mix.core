using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixSystem;
using Mix.Domain.Core.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixConfigurations
{
    public class UpdateViewModel : ViewModelBase<MixCmsContext, MixConfiguration, UpdateViewModel>
    {
        #region Properties

        #region Models

        [Required]
        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("dataType")]
        public MixDataType DataType { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy{ get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain", Specificulture) ?? "/"; } }

        [JsonProperty("property")]
        public DataValueViewModel Property { get; set; }
        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixConfiguration model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixConfiguration ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Value = Property.Value;
            DataType = Property.DataType;
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Cultures = LoadCultures(Specificulture, _context, _transaction);
            this.Cultures.ForEach(c => c.IsSupported = true);
            Property = new DataValueViewModel() { DataType = DataType, Value = Value, Name = Keyword };
        }

        #endregion Overrides
        #region Expand
        List<SupportedCulture> LoadCultures(string initCulture = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getCultures = SystemCultureViewModel.Repository.GetModelList(_context, _transaction);
            var result = new List<SupportedCulture>();
            if (getCultures.IsSucceed)
            {
                foreach (var culture in getCultures.Data)
                {
                    result.Add(
                        new SupportedCulture()
                        {
                            Icon = culture.Icon,
                            Specificulture = culture.Specificulture,
                            Alias = culture.Alias,
                            FullName = culture.FullName,
                            Description = culture.FullName,
                            Id = culture.Id,
                            Lcid = culture.Lcid,
                            IsSupported = culture.Specificulture == initCulture || _context.MixConfiguration.Any(p => p.Keyword == Keyword && p.Specificulture == culture.Specificulture)
                        });

                }
            }
            return result;
        }

        #endregion
    }
}
