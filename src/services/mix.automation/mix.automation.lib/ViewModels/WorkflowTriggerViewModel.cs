using Mix.Automation.Lib.Entities;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.ViewModels
{
    public sealed class WorkflowTriggerViewModel : WorkflowViewModelBase<WorkflowDbContext, WorkflowTrigger, int, WorkflowTriggerViewModel>
    {
        #region Contructors

        public WorkflowTriggerViewModel()
        {
        }

        public WorkflowTriggerViewModel(WorkflowDbContext context) : base(context)
        {
        }

        public WorkflowTriggerViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public WorkflowTriggerViewModel(WorkflowTrigger entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties
        public bool IsSuccess { get; set; }
        public int WorkflowId { get; set; }
        public JObject? Input { get; set; }

        public List<WorkflowActionDataViewModel> Actions { get; set; }
        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Actions = await WorkflowActionDataViewModel.GetRepository(UowInfo, CacheService).
                GetSortedListAsync(m => m.TriggerId == Id, nameof(WorkflowAction.Index), Heart.Enums.SortDirection.Asc, cancellationToken);
        }
        protected override async Task SaveEntityRelationshipAsync(WorkflowTrigger parentEntity, CancellationToken cancellationToken = default)
        {
            foreach (var action in Actions)
            {
                action.TriggerId = parentEntity.Id;
                action.SetUowInfo(UowInfo, CacheService);
                await action.SaveAsync(cancellationToken);
            }
        }
        #endregion
    }
}
