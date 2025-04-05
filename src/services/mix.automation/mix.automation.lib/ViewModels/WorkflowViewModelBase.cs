using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Mix.Automation.Lib.Entities;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.ViewModels
{
    public abstract class WorkflowViewModelBase<TDbContext, TEntity, TPrimaryKey, TView> : SimpleViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
        where TView : WorkflowViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {

        #region Contructors

        public WorkflowViewModelBase()
        {
        }

        public WorkflowViewModelBase(TDbContext context) : base(context)
        {
        }

        public WorkflowViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public WorkflowViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string? language = null, int? cultureId = null)
        {
            if (CreatedDateTime == default)
            {
                CreatedDateTime = DateTime.UtcNow;
                LastModified = DateTime.UtcNow;
            }
        }
        #endregion
    }
}
