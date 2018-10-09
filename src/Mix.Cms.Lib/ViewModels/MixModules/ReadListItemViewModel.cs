using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixModules
{
    public class ReadListItemViewModel : ViewModelBase<MixCmsContext, MixModule, ReadListItemViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public string Domain { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public ModuleType Type { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }
        #endregion Models

        #endregion Properties

        #region Contructors

        public ReadListItemViewModel() : base()
        {
        }

        public ReadListItemViewModel(MixModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Domain = MixService.GetConfig<string>("Domain", Specificulture) ?? "/";
            if (Image != null && (Image.IndexOf("http") == -1 && Image[0] != '/'))
            {
                ImageUrl = CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
            }
            else
            {
                ImageUrl = Image;
            }
        }

        public override Task<bool> ExpandViewAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Domain = MixService.GetConfig<string>("Domain", Specificulture) ?? "/";
            if (Image != null && (Image.IndexOf("http") == -1 && Image[0] != '/'))
            {
                ImageUrl = CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
            }
            else
            {
                ImageUrl = Image;
            }
            return base.ExpandViewAsync(_context, _transaction);
        }

        #endregion
    }
}
