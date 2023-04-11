using Mix.Database.Entities.AuditLog;
using Mix.Lib.Services;

namespace Mix.Lib.ViewModels
{
    public sealed class AuditLogViewModel
        : ViewModelBase<AuditLogDbContext, AuditLog, Guid, AuditLogViewModel>
    {
        #region Properties
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public JObject Body { get; set; }
        public bool Success { get; set; }
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
