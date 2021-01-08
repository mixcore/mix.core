﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixModulePosts
{
    public class ReadViewModel
       : ViewModelBase<MixCmsContext, MixModulePost, ReadViewModel>
    {
        public ReadViewModel(MixModulePost model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public ReadViewModel() : base()
        {
        }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }
        [JsonProperty("postId")]
        public int PostId { get; set; }

        [JsonProperty("moduleId")]
        public int ModuleId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

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
        #region Views

        [JsonProperty("post")]
        public MixPosts.ReadListItemViewModel Post { get; set; }

        [JsonProperty("module")]
        public MixModules.ReadListItemViewModel Module { get; set; }

        #endregion Views

        #region overrides
        public override void Validate(MixCmsContext _context, IDbContextTransaction _transaction)
        {
            base.Validate(_context, _transaction);
            if (IsValid)
            {
                IsValid = !_context.MixModulePost.Any(m => m.PostId == PostId && m.ModuleId == ModuleId
                    && m.Id != Id && m.Specificulture == Specificulture);
                if (!IsValid)
                {
                    Errors.Add("Existed");
                }
            }
        }
        public override MixModulePost ParseModel(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            if (Id == 0)
            {
                Id = Repository.Max(c => c.Id, _context, _transaction).Data + 1;
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseModel(_context, _transaction);
        }

        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getPost = MixPosts.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == PostId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getPost.IsSucceed)
            {
                Post = getPost.Data;
            }

            var getModule = MixModules.ReadListItemViewModel.Repository.GetSingleModel(p => p.Id == ModuleId && p.Specificulture == Specificulture
               , _context: _context, _transaction: _transaction
           );
            if (getModule.IsSucceed)
            {
                Module = getModule.Data;
            }
        }

        #endregion overrides

        #region Expand

        public static RepositoryResponse<List<MixModulePosts.ReadViewModel>> GetModulePostNavAsync(int postId, string specificulture
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var navCategoryPostViewModels = context.MixModule.Include(cp => cp.MixModulePost).Where(a => a.Specificulture == specificulture
                    && (a.Type == MixModuleType.ListPost)
                    )
                    .AsEnumerable()
                    .Select(p => new MixModulePosts.ReadViewModel(
                        new MixModulePost()
                        {
                            PostId = postId,
                            ModuleId = p.Id,
                            Specificulture = specificulture
                        },
                        _context, _transaction)
                    {
                        IsActived = p.MixModulePost.Any(cp => cp.PostId == postId && cp.Specificulture == specificulture),
                        Description = p.Title
                    });
                return new RepositoryResponse<List<ReadViewModel>>()
                {
                    IsSucceed = true,
                    Data = navCategoryPostViewModels.ToList()
                };
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                if (isRoot)
                {
                    transaction.Rollback();
                }
                return new RepositoryResponse<List<MixModulePosts.ReadViewModel>>()
                {
                    IsSucceed = true,
                    Data = null,
                    Exception = ex
                };
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        #endregion Expand
    }
}