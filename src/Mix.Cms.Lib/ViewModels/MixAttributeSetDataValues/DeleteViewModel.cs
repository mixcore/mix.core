using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetValues
{
    public class DeleteViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetValue, DeleteViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributeFieldId")]
        public int AttributeFieldId { get; set; }

        [JsonProperty("regex")]
        public string Regex { get; set; }

        [JsonProperty("dataType")]
        public int DataType { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("attributeFieldName")]
        public string AttributeFieldName { get; set; }

        [JsonProperty("attributeSetName")]
        public string AttributeSetName { get; set; }

        [JsonProperty("booleanValue")]
        public bool? BooleanValue { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty("dataId")]
        public string DataId { get; set; }

        [JsonProperty("dateTimeValue")]
        public DateTime? DateTimeValue { get; set; }

        [JsonProperty("doubleValue")]
        public double? DoubleValue { get; set; }

        [JsonProperty("integerValue")]
        public int? IntegerValue { get; set; }

        [JsonProperty("stringValue")]
        public string StringValue { get; set; }

        [JsonProperty("encryptValue")]
        public string EncryptValue { get; set; }

        [JsonProperty("encryptKey")]
        public string EncryptKey { get; set; }

        [JsonProperty("encryptType")]
        public int EncryptType { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public DeleteViewModel() : base()
        {
            IsCache = false;
        }

        public DeleteViewModel(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
            IsCache = false;
        }

        #endregion Contructors

        #region Overrides

        public override Task RemoveCache(MixAttributeSetValue model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            Task result = null;
            var tasks = new List<Task>();
            try
            {
                var navs = context.MixRelatedAttributeData.Where(m => m.Id == model.DataId && m.Specificulture == model.Specificulture);
                if (navs != null)
                {
                    foreach (var item in navs)
                    {
                        switch (item.ParentType)
                        {
                            case 0:
                                break;

                            case 1:
                                break;

                            case 2:
                                int.TryParse(item.ParentId, out int postId);
                                var post = context.MixPost.FirstOrDefault(m => m.Id == postId && m.Specificulture == Specificulture);
                                if (post != null)
                                {
                                    tasks.Add(MixPosts.ReadViewModel.Repository.RemoveCache(post, context, transaction));
                                }
                                break;

                            case 3:
                                int.TryParse(item.ParentId, out int pageId);
                                var page = context.MixPage.FirstOrDefault(m => m.Id == pageId && m.Specificulture == Specificulture);
                                if (page != null)
                                {
                                    tasks.Add(MixPages.ReadViewModel.Repository.RemoveCache(page, context, transaction));
                                }
                                break;

                            case 4:
                                int.TryParse(item.ParentId, out int moduleId);
                                var module = context.MixModule.FirstOrDefault(m => m.Id == moduleId && m.Specificulture == Specificulture);
                                if (module != null)
                                {
                                    tasks.Add(MixModules.ReadListItemViewModel.Repository.RemoveCache(module, context, transaction));
                                }
                                break;

                            case 5:
                                break;

                            default:
                                break;
                        }
                    }
                }
                tasks.Add(base.RemoveCache(model, context, transaction));
                result = Task.WhenAll(tasks);
                result.Wait();
                return result;
            }
            catch (Exception ex)
            {
                UnitOfWorkHelper<MixCmsContext>.HandleException<UpdateViewModel>(ex, isRoot, transaction);
                return Task.FromException(ex);
            }
            finally
            {
                if (isRoot && (result.Status == TaskStatus.RanToCompletion || result.Status == TaskStatus.Canceled || result.Status == TaskStatus.Faulted))
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        #endregion Overrides
    }
}