using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPortalPages
{
    public class UpdateViewModel
       : ViewModelBase<MixCmsContext, MixPortalPage, UpdateViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("textKeyword")]
        public string TextKeyword { get; set; }

        [JsonProperty("textDefault")]
        public string TextDefault { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

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

        [JsonProperty("childNavs")]
        public List<MixPortalPagePortalPages.UpdateViewModel> ChildNavs { get; set; } = new List<MixPortalPagePortalPages.UpdateViewModel>();

        [JsonProperty("parentNavs")]
        public List<MixPortalPagePortalPages.UpdateViewModel> ParentNavs { get; set; } = new List<MixPortalPagePortalPages.UpdateViewModel>();

        #endregion Views

        #endregion Properties

        #region Contructors

        public UpdateViewModel() : base()
        {
        }

        public UpdateViewModel(MixPortalPage model, MixCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override MixPortalPage ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = UpdateViewModel.Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            var navParent = ParentNavs?.FirstOrDefault(p => p.IsActived);

            if (navParent != null)
            {
                Level = navParent.Level + 1;
            }
            else
            {
                Level = 0;
            }

            if (ChildNavs != null)
            {
                ChildNavs.Where(c => c.IsActived).ToList().ForEach(c => c.PortalPage.Level = Level + 1);
            }

            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.ParentNavs = GetParentNavs(_context, _transaction);
            this.ChildNavs = GetChildNavs(_context, _transaction);
        }

        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixPortalPage parent, MixCmsContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool> { IsSucceed = true };

            if (result.IsSucceed)
            {
                foreach (var item in ParentNavs)
                {
                    item.PageId = parent.Id;
                    var startId = Lib.ViewModels.MixPortalPagePortalPages.UpdateViewModel.Repository.Max(m => m.Id, _context, _transaction).Data + 1;
                    if (item.IsActived)
                    {
                        if (item.Id == 0)
                        {
                            item.Id = startId;
                            startId += 1;
                        }
                        var saveResult = await item.SaveModelAsync(false, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            Errors.AddRange(saveResult.Errors);
                        }
                    }
                    else
                    {
                        var saveResult = await item.RemoveModelAsync(true, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            Errors.AddRange(saveResult.Errors);
                        }
                    }
                }
            }

            if (result.IsSucceed)
            {
                foreach (var item in ChildNavs)
                {
                    item.ParentId = parent.Id;
                    var startId = Lib.ViewModels.MixPortalPagePortalPages.UpdateViewModel.Repository.Max(m => m.Id, _context, _transaction).Data + 1;
                    if (item.IsActived)
                    {
                        if (item.Id == 0)
                        {
                            item.Id = startId;
                            startId += 1;
                        }

                        var saveResult = await item.SaveModelAsync(true, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            Errors.AddRange(saveResult.Errors);
                        }
                    }
                    else
                    {
                        var saveResult = await item.RemoveModelAsync(true, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Exception = saveResult.Exception;
                            Errors.AddRange(saveResult.Errors);
                        }
                    }
                }
            }
            return result;
        }

        public override async Task<RepositoryResponse<bool>> RemoveRelatedModelsAsync(UpdateViewModel view, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var removeNavs = await MixPortalPagePortalPages.UpdateViewModel.Repository.RemoveListModelAsync(false, p => p.PageId == Id || p.ParentId == Id, _context, _transaction);
            var result = new RepositoryResponse<bool>()
            {
                IsSucceed = true,
                Errors = removeNavs.Errors,
                Exception = removeNavs.Exception
            };
            return result;
        }

        #endregion Overrides

        #region Expands

        public List<MixPortalPagePortalPages.UpdateViewModel> GetParentNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixPortalPagePortalPages.UpdateViewModel.Repository.GetModelListBy(
                m => m.PageId == Id, context, transaction).Data;
            result.ForEach(nav =>
            {
                nav.IsActived = true;
            });

            //var activeIds = result.Select(m => m.PageId).ToList();

            //var query = context.MixPortalPage
            //    .Include(cp => cp.MixPortalPageNavigationParent)
            //    .Where(PortalPage =>
            //        // not load current active parent
            //        !activeIds.Any(p => p == PortalPage.Id) &&
            //        // not load current page or page already have another parent
            //        PortalPage.Id != Id
            //    )
            //    .AsEnumerable()
            //    .Select(PortalPage =>
            //        new MixPortalPagePortalPages.ReadViewModel()
            //        {
            //            PageId = Id,
            //            ParentId = PortalPage.Id,
            //            Description = PortalPage.TextDefault,
            //            Level = PortalPage.Level
            //        }
            //    );
            //result.AddRange(query.ToList());
            return result.OrderBy(m => m.Priority).ToList();
        }

        public List<MixPortalPagePortalPages.UpdateViewModel> GetChildNavs(MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = MixPortalPagePortalPages.UpdateViewModel.Repository.GetModelListBy(
               m => m.ParentId == Id, context, transaction).Data;
            result.ForEach(nav =>
            {
                nav.IsActived = true;
            });
            //var activeIds = result.Select(m => m.PageId).ToList();

            //var query = context.MixPortalPage
            //    .Include(cp => cp.MixPortalPageNavigationParent)
            //    .Where(PortalPage =>
            //            // not load current active parent
            //            !activeIds.Any(p => p == PortalPage.Id) &&
            //            // not load current page or page already have another parent
            //            PortalPage.Id != Id
            //        )
            //    .AsEnumerable()
            //    .Select(PortalPage =>
            //    new MixPortalPagePortalPages.ReadViewModel(
            //          new MixPortalPageNavigation()
            //          {
            //              PageId = PortalPage.Id,
            //              ParentId = Id,
            //              Description = PortalPage.TextDefault,
            //          }, context, transaction));

            //result.AddRange(query.ToList());
            return result.OrderBy(m => m.Priority).ToList();
        }

        #endregion Expands
    }
}