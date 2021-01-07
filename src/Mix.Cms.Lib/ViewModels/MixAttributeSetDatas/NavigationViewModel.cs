using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDataValues;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class NavigationViewModel
      : ViewModelBase<MixCmsContext, MixAttributeSetData, NavigationViewModel>
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

        public List<MixAttributeSetValues.NavigationViewModel> Values { get; set; }

        public List<MixAttributeFields.ReadViewModel> Fields { get; set; }

        [JsonProperty("data")]
        public JObject Obj { get; set; }

        [JsonProperty("nav")]
        public Navigation Nav
        {
            get
            {
                if (AttributeSetName == MixConstants.AttributeSetName.NAVIGATION && Obj != null)
                {
                    return Obj.ToObject<Navigation>();
                }
                return null;
            }
        }

        #endregion Views

        #endregion Properties

        #region Contructors

        public NavigationViewModel() : base()
        {
        }

        public NavigationViewModel(MixAttributeSetData model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
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

        #region Async

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixAttributeSetData parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (result.IsSucceed)
            {
                foreach (var item in Values)
                {
                    if (result.IsSucceed)
                    {
                        if (Fields.Any(f => f.Id == item.AttributeFieldId))
                        {
                            item.Priority = item.Field.Priority;
                            item.DataId = parent.Id;
                            item.Specificulture = parent.Specificulture;
                            var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                            ViewModelHelper.HandleResult(saveResult, ref result);
                        }
                        else
                        {
                            var delResult = await item.RemoveModelAsync(false, _context, _transaction);
                            ViewModelHelper.HandleResult(delResult, ref result);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }

        #endregion Async

        #endregion Overrides



        #region Expands
       
        #endregion Expands
    }

    public class Navigation
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("menu_items")]
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        [JsonProperty("actived_menu_items")]
        public List<MenuItem> ActivedMenuItems { get; set; } = new List<MenuItem>();

        [JsonProperty("actived_menu_item")]
        public MenuItem ActivedMenuItem { get; set; }
    }

    public class MenuItem
    {
        [JsonIgnore]
        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("classes")]
        public string Classes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("target_id")]
        public string TargetId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("menu_items")]
        public List<MenuItem> MenuItems { get; set; }

        public T Property<T>(string fieldName)
        {
            if (Data != null)
            {
                var field = Data.GetValue(fieldName);
                if (field != null)
                {
                    return field.Value<T>();
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }
    }
}