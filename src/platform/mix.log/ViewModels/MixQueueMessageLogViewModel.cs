using Mix.Constant.Enums;
using Mix.Database.Entities.Queue;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Newtonsoft.Json.Linq;

namespace Mix.Log.Lib.ViewModels
{
    public sealed class MixQueueMessageLogViewModel
        : ViewModelBase<MixQueueDbContext, MixQueueMessageLog, Guid, MixQueueMessageLogViewModel>
    {
        #region Properties
        public Guid? QueueMessageId { get; set; }
        public string TopicId { get; set; }
        public string SubscriptionId { get; set; }
        public string Action { get; set; }
        public string StringData { get; set; }
        public JObject ObjectData { get; set; }
        public JObject Exception { get; set; }
        public JArray Subscriptions { get; set; }

        public string DataTypeFullName { get; set; }
        public string Note { get; set; }
        public MixQueueMessageLogState State { get; set; }
        public int TenantId { get; set; }
        #endregion

        #region Constructors

        public MixQueueMessageLogViewModel()
        {
        }

        public MixQueueMessageLogViewModel(MixQueueMessageLog entity, UnitOfWorkInfo? uowInfo)
            : base(entity, uowInfo)
        {
        }

        public MixQueueMessageLogViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion

        #region Expands

        #endregion
    }
}
