using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class ODataUpdateViewModel
       : ODataViewModelBase<MixCmsContext, MixRelatedAttributeData, ODataUpdateViewModel>
    {
        public ODataUpdateViewModel(MixRelatedAttributeData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ODataUpdateViewModel() : base()
        {
        }

        #region Model

        /*
         * Attribute Set Data Id
         */

        [JsonProperty("id")]
        public string Id { get; set; }

        /*
         * Parent Id: PostId / PageId / Module Id / Data Id / Attr Set Id
         */

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentType")]
        public int ParentType { get; set; } // cannot use mixenum for odata request

        [JsonProperty("attributeSetId")]
        public int AttributeSetId { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion Model

        #region Views

        [JsonProperty("data")]
        public MixAttributeSetDatas.UpdateViewModel Data { get; set; }

        #endregion Views

        #region overrides

        public override MixRelatedAttributeData ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getData = MixAttributeSetDatas.UpdateViewModel.Repository.GetSingleModel(p => p.Id == Id && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getData.IsSucceed)
            {
                Data = getData.Data;
            }
            AttributeSetName = _context.MixAttributeSet.FirstOrDefault(m => m.Id == AttributeSetId)?.Name;
        }

        //public override List<Task> GenerateRelatedData(MixCmsContext context, IDbContextTransaction transaction)
        //{
        //    var tasks = new List<Task>();
        //    // MixEnums.MixAttributeSetDataType
        //    switch (ParentType)
        //    {
        //        case 0:
        //        case 1:
        //        case 5:
        //            var attrDatas = context.MixAttributeSetData.Where(m => m.Specificulture == Specificulture && m.Id == ParentId);
        //            foreach (var item in attrDatas)
        //            {
        //                tasks.Add(Task.Run(() =>
        //                {
        //                    var updModel = new MixAttributeSetDatas.ReadViewModel(item, context, transaction);
        //                    updModel.GenerateCache(item, updModel);
        //                }));
        //            }
        //            break;

        //        case 2:
        //            int.TryParse(ParentId, out int postId);
        //            var post = context.MixPost.First(m => m.Specificulture == Specificulture && m.Id == postId);
        //            if (post != null)
        //            {
        //                tasks.Add(Task.Run(() =>
        //                {
        //                    var updModel = new MixPosts.ReadViewModel(post, context, transaction);
        //                    updModel.GenerateCache(post, updModel);
        //                }));
        //            }
        //            break;

        //        case 3:
        //            int.TryParse(ParentId, out int pageId);
        //            var page = context.MixPage.First(m => m.Specificulture == Specificulture && m.Id == pageId);
        //            if (page != null)
        //            {
        //                tasks.Add(Task.Run(() =>
        //                {
        //                    var updModel = new MixPages.ReadViewModel(page, context, transaction);
        //                    updModel.GenerateCache(page, updModel);
        //                }));
        //            }
        //            break;

        //        case 4:
        //            int.TryParse(ParentId, out int moduleId);
        //            var module = context.MixModule.First(m => m.Specificulture == Specificulture && m.Id == moduleId);
        //            if (module != null)
        //            {
        //                tasks.Add(Task.Run(() =>
        //                {
        //                    var updModel = new MixModules.ReadListItemViewModel(module, context, transaction);
        //                    updModel.GenerateCache(module, updModel);
        //                }));
        //            }
        //            break;

        //        default:
        //            break;
        //    }
        //    return tasks;
        //}

        #endregion overrides
    }
}