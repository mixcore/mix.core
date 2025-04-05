using Mix.Automation.Lib.Entities;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.ViewModels
{
    public sealed class WorkflowViewModel : WorkflowViewModelBase<WorkflowDbContext, Workflow, int, WorkflowViewModel>
    {
        #region Contructors

        public WorkflowViewModel()
        {
        }

        public WorkflowViewModel(WorkflowDbContext context) : base(context)
        {
        }

        public WorkflowViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public WorkflowViewModel(Workflow entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Title { get; set; }
        public string Description { get; set; }

        public List<WorkflowActionViewModel> Actions { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Actions = await WorkflowActionViewModel.GetRepository(UowInfo, CacheService).GetAllAsync(m => m.WorkflowId == Id, cancellationToken);
        }

        #endregion
    }
}
