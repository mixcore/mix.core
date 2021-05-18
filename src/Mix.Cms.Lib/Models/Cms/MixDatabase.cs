using Mix.Cms.Lib.Enums;
using Mix.Heart.Infrastructure.Entities;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixDatabase: AuditedEntity
    {
        public MixDatabase()
        {
        }

        public int Id { get; set; }
        public MixDatabaseType Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormTemplate { get; set; }
        public string EdmTemplate { get; set; }
        public string EdmSubject { get; set; }
        public string EdmFrom { get; set; }
        public bool? EdmAutoSend { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
    }
}