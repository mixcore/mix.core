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
    public sealed class WorkflowActionViewModel : WorkflowViewModelBase<WorkflowDbContext, WorkflowAction, int, WorkflowActionViewModel>
    {
        #region Contructors

        public WorkflowActionViewModel()
        {
        }

        public WorkflowActionViewModel(WorkflowDbContext context) : base(context)
        {
        }

        public WorkflowActionViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public WorkflowActionViewModel(WorkflowAction entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties
        public ActionType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InputPath { get; set; }
        public int Index { get; set; }
        public int WorkflowId { get; set; }
        public JObject? Request { get; set; }
        public JObject? Body { get; set; }
        #endregion
    }
}
