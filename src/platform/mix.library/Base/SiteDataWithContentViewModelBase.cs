﻿using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Lib.Base
{
    public abstract class SiteDataWithContentViewModelBase
        <TDbContext, TEntity, TPrimaryKey, TContentEntity, TContent>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TContentEntity : MultilanguageContentBase<TPrimaryKey>
        where TContent : MultilanguageContentViewModelBase<TDbContext, TContentEntity, TPrimaryKey>
    {
        #region Contructors
        protected SiteDataWithContentViewModelBase()
        {
        }

        protected SiteDataWithContentViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected SiteDataWithContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected SiteDataWithContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }

        public virtual string Image { get; set; }

        public int MixTenantId { get; set; }

        public List<TContent> Contents { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView(UnitOfWorkInfo uowInfo = null)
        {
            UowInfo ??= uowInfo;
            using var _contentQueryRepository = new QueryRepository<TDbContext, TContentEntity, TPrimaryKey>(UowInfo);

            Contents = await _contentQueryRepository.GetListViewAsync<TContent>(
                        m => m.ParentId.Equals(Id));
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
            MixTenantId = 1;
        }

        protected override async Task SaveEntityRelationshipAsync(TEntity parentEntity)
        {
            if (Contents != null)
            {
                foreach (var item in Contents)
                {
                    item.ParentId = parentEntity.Id;
                    await item.SaveAsync(UowInfo);
                }
            }
        }

        protected override async Task DeleteHandlerAsync()
        {
            Repository<TDbContext, TContentEntity, TPrimaryKey> contentRepo = new(UowInfo);
            await contentRepo.DeleteManyAsync(m => m.ParentId.Equals(Id));
            await base.DeleteHandlerAsync();
        }
        #endregion
    }
}