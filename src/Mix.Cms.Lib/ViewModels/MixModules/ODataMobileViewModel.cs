using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixModules
{
    public class ODataMobileViewModel : ViewModelBase<MixCmsContext, MixModule, ODataMobileViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("formTemplate")]
        public string FormTemplate { get; set; }

        [JsonProperty("edmTemplate")]
        public string EdmTemplate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("domain")]
        public string Domain { get { return MixService.GetConfig<string>("Domain"); } }

        [JsonProperty("fields")]
        public string Fields { get; set; }

        [JsonProperty("type")]
        public MixModuleType Type { get; set; }

        [JsonProperty("status")]
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }

        #endregion Models

        [JsonProperty("imageUrl")]
        public string ImageUrl {
            get {
                if (!string.IsNullOrWhiteSpace(Image) && (Image.IndexOf("http") == -1) && Image[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Image
                });
                }
                else
                {
                    return Image;
                }
            }
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl {
            get {
                if (Thumbnail != null && Thumbnail.IndexOf("http") == -1 && Thumbnail[0] != '/')
                {
                    return CommonHelper.GetFullPath(new string[] {
                    Domain,  Thumbnail
                });
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        #endregion Properties

        #region Contructors

        public ODataMobileViewModel() : base()
        {
        }

        public ODataMobileViewModel(MixModule model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
        }

        public override Task<bool> ExpandViewAsync(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            return base.ExpandViewAsync(_context, _transaction);
        }

        #endregion Overrides

        #region Expands

        public static async Task<RepositoryResponse<JObject>> SaveByModuleName(string culture, string createdBy, string name, string formName, JObject obj
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var getModule = await Repository.GetSingleModelAsync(m => m.Specificulture == culture && m.Name == name, context, transaction);
                string dataId = obj["id"]?.Value<string>();
                if (getModule.IsSucceed)
                {
                    // Get Attribute set
                    var getAttrSet = await Mix.Cms.Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModelAsync(m => m.Name == formName, context, transaction);
                    if (getAttrSet.IsSucceed)
                    {
                        // Save attr data + navigation
                        Lib.ViewModels.MixAttributeSetDatas.ODataMobileViewModel data = new Lib.ViewModels.MixAttributeSetDatas.ODataMobileViewModel()
                        {
                            Id = dataId,
                            CreatedBy = createdBy,
                            AttributeSetId = getAttrSet.Data.Id,
                            AttributeSetName = getAttrSet.Data.Name,
                            Specificulture = culture,
                            Data = obj
                        };

                        // Create navigation module - attr data
                        var getNavigation = await Lib.ViewModels.MixRelatedAttributeDatas.ODataDeleteViewModel.Repository.GetSingleModelAsync(
                            m => m.ParentId == getModule.Data.Id.ToString() && m.ParentType == (int)MixEnums.MixAttributeSetDataType.Module && m.Specificulture == culture
                            , context, transaction);
                        if (!getNavigation.IsSucceed)
                        {
                            data.RelatedData.Add(new Lib.ViewModels.MixRelatedAttributeDatas.ODataMobileViewModel()
                            {
                                ParentId = getModule.Data.Id.ToString(),
                                Specificulture = culture,
                                ParentType = MixAttributeSetDataType.Module
                            });
                        }
                        var portalResult = await data.SaveModelAsync(true, context, transaction);
                        UnitOfWorkHelper<MixCmsContext>.HandleTransaction(portalResult.IsSucceed, isRoot, transaction);
                        if (portalResult.IsSucceed)
                        {
                            _ = SendEdm(getModule.Data.Specificulture, getModule.Data.EdmTemplate, obj);
                        }
                        return new RepositoryResponse<JObject>()
                        {
                            IsSucceed = portalResult.IsSucceed,
                            Data = portalResult.Data?.Data,
                            Exception = portalResult.Exception,
                            Errors = portalResult.Errors
                        };
                    }
                    else
                    {
                        return new RepositoryResponse<JObject>()
                        {
                            IsSucceed = false,
                            Status = (int)MixEnums.ResponseStatus.BadRequest
                        };
                    }
                }
                else
                {
                    return new RepositoryResponse<JObject>()
                    {
                        IsSucceed = false,
                        Status = (int)MixEnums.ResponseStatus.BadRequest
                    };
                }
            }
            catch (Exception ex)
            {
                return (UnitOfWorkHelper<MixCmsContext>.HandleException<JObject>(ex, isRoot, transaction));
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }

        private static Task SendEdm(string culture, string template, JObject data)
        {
            return Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(data["email"].Value<string>()))
                {
                    var getEdm = MixTemplates.UpdateViewModel.GetTemplateByPath(template, culture);
                    if (getEdm.IsSucceed && !string.IsNullOrEmpty(getEdm.Data.Content))
                    {
                        string body = getEdm.Data.Content;
                        foreach (var prop in data.Properties())
                        {
                            body = body.Replace($"[[{prop.Name}]]", prop.Value<string>());
                        }
                        MixService.SendMail("Invitation", getEdm.Data.Content, data["email"].Value<string>());
                    }
                }
            });
        }

        #endregion Expands
    }
}