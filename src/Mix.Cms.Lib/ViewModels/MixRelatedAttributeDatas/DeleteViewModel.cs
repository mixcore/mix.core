using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class DeleteViewModel
        : ViewModelBase<MixCmsContext, MixRelatedAttributeData, DeleteViewModel>
    {
        #region Properties

        #region Model
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("parentId")]
        public string ParentId { get; set; }
        [JsonProperty("parentType")]
        public int ParentType { get; set; }
        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }
        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        #endregion

        #region Views

        #endregion Views

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
        }

        public DeleteViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixRelatedAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        #endregion Overrides
    }
}
