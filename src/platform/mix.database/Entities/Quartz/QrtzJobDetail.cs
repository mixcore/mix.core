using System.Collections.Generic;

namespace Mix.Database.Entities.Quartz
{
    public partial class QrtzJobDetail
    {
        public QrtzJobDetail()
        {
            QrtzTriggers = new HashSet<QrtzTrigger>();
        }

        public string SchedName { get; set; }
        public string JobName { get; set; }
        public string JobGroup { get; set; }
        public string Description { get; set; }
        public string JobClassName { get; set; }
        public bool IsDurable { get; set; }
        public bool IsNonconcurrent { get; set; }
        public bool IsUpdateData { get; set; }
        public bool RequestsRecovery { get; set; }
        public byte[] JobData { get; set; }

        public virtual ICollection<QrtzTrigger> QrtzTriggers { get; set; }
    }
}
