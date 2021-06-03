using System;

namespace Mix.Database.Entities.Base
{
    public interface IExternalDataEntity
    {
        Guid ExternalDataId { get; set; }
        string ExternalDatbaseName { get; set; }
    }
}
