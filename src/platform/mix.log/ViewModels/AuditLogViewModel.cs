using Mix.Database.Entities.AuditLog;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Newtonsoft.Json.Linq;

namespace Mix.Log.Lib.ViewModels
{
    public sealed class AuditLogViewModel
        : ViewModelBase<AuditLogDbContext, AuditLog, Guid, AuditLogViewModel>
    {
        #region Properties
        public bool Success { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public JObject Body { get; set; }
        public JObject Response { get; set; }
        public JObject Exception { get; set; }
        #endregion

        #region Constructors

        public AuditLogViewModel()
        {
        }

        public AuditLogViewModel(AuditLog entity, UnitOfWorkInfo? uowInfo)
            : base(entity, uowInfo)
        {
        }

        public AuditLogViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion

        #region Expands

        #endregion
    }
}
