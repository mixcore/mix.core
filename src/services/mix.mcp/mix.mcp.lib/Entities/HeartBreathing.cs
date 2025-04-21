using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mix.Database.Entities;
using Mix.Heart.Entities;
using NpgsqlTypes;

namespace Mix.Mcp.Lib.Entities
{
    [Table("heart_breathing")]
    public class HeartBreathing : EntityBase<int>
    {
        [Column("sensor_data", TypeName = "jsonb")]
        public NpgsqlJsonDocument SensorData { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("device_id")]
        public string DeviceId { get; set; }

        [Column("patient_id")]
        public string PatientId { get; set; }

        [Column("heart_rate")]
        public double? HeartRate { get; set; }

        [Column("breathing_rate")]
        public double? BreathingRate { get; set; }

        [Column("oxygen_level")]
        public double? OxygenLevel { get; set; }

        [Column("is_alert")]
        public bool IsAlert { get; set; }

        [Column("alert_type")]
        public string AlertType { get; set; }

        public HeartBreathing()
        {
            Timestamp = DateTime.UtcNow;
            CreatedDateTime = DateTime.UtcNow;
            Status = Heart.Enums.MixContentStatus.Published;
            Priority = 0;
            IsDeleted = false;
            IsAlert = false;
            AlertType = string.Empty;
        }
    }
} 