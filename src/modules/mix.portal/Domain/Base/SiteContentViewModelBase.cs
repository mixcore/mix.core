﻿using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Portal.Domain.Base
{
    public abstract class SiteContentViewModelBase<TDbContext, TEntity, TPrimaryKey> : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
         where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Contructors

        public SiteContentViewModelBase()
        {

        }

        protected SiteContentViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected SiteContentViewModelBase(TEntity entity) : base(entity)
        {
        }

        protected SiteContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Properties

        public string Specificulture { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public TPrimaryKey ParentId { get; set; }
        public int MixCultureId { get; set; }

        #endregion

    }
}