using Mix.Automation.Lib.Entities;
using Mix.Automation.Lib.Enums;
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
    public sealed class WorkflowActionDataViewModel : WorkflowViewModelBase<WorkflowDbContext, WorkflowActionData, int, WorkflowActionDataViewModel>
    {
        #region Contructors

        public WorkflowActionDataViewModel()
        {
        }

        public WorkflowActionDataViewModel(WorkflowDbContext context) : base(context)
        {
        }

        public WorkflowActionDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public WorkflowActionDataViewModel(WorkflowActionData entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties
        public bool IsSuccess { get; set; }
        public ActionType ActionType { get; set; }
        public ActionStatus? ActionStatus { get; set; }
        public double Duration { get; set; }
        public int TriggerId { get; set; }
        public int ActionId { get; set; }
        public int Index { get; set; }
        public JObject? Request { get; set; }
        public JObject? Body { get; set; }
        public JObject? Response { get; set; }
        public JObject? Exception { get; set; }

        public WorkflowActionViewModel Action { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Action = await WorkflowActionViewModel.GetRepository(UowInfo, CacheService)
                .GetSingleAsync(m => m.Id == ActionId);
        }

        #endregion

    }
}
