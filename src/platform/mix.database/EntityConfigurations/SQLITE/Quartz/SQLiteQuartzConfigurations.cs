using Mix.Database.Entities.Quartz;

namespace Mix.Database.EntityConfigurations.SQLITE.Quartz
{
    public class SQLiteQuartzConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<QrtzBlobTrigger>(entity =>
                {
                    entity.HasKey(e => new
                    {
                        e.SchedName,
                        e.TriggerName,
                        e.TriggerGroup
                    });

                    entity.ToTable("QRTZ_BLOB_TRIGGERS");

                    entity.Property(e => e.SchedName)
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    entity.Property(e => e.TriggerName)
                                .HasColumnType("NVARCHAR(150)")
                                .HasColumnName("TRIGGER_NAME");

                    entity.Property(e => e.TriggerGroup)
                                .HasColumnType("NVARCHAR(150)")
                                .HasColumnName("TRIGGER_GROUP");

                    entity.Property(e => e.BlobData).HasColumnName("BLOB_DATA");

                });

            modelBuilder.Entity<QrtzCalendar>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.CalendarName });

                entity.ToTable("QRTZ_CALENDARS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.CalendarName)
                    .HasColumnType("NVARCHAR(200)")
                    .HasColumnName("CALENDAR_NAME");

                entity.Property(e => e.Calendar)
                    .IsRequired()
                    .HasColumnName("CALENDAR");
            });

            modelBuilder.Entity<QrtzCronTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_CRON_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.CronExpression)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(250)")
                    .HasColumnName("CRON_EXPRESSION");

                entity.Property(e => e.TimeZoneId)
                    .HasColumnType("NVARCHAR(80)")
                    .HasColumnName("TIME_ZONE_ID");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzCronTrigger)
                    .HasForeignKey<QrtzCronTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup });
            });

            modelBuilder.Entity<QrtzFiredTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.EntryId });

                entity.ToTable("QRTZ_FIRED_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.EntryId)
                    .HasColumnType("NVARCHAR(140)")
                    .HasColumnName("ENTRY_ID");

                entity.Property(e => e.FiredTime)
                    .HasColumnType("BIGINT")
                    .HasColumnName("FIRED_TIME");

                entity.Property(e => e.InstanceName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(200)")
                    .HasColumnName("INSTANCE_NAME");

                entity.Property(e => e.IsNonconcurrent)
                    .HasColumnType("BIT")
                    .HasColumnName("IS_NONCONCURRENT");

                entity.Property(e => e.JobGroup)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.JobName)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.Priority).HasColumnName("PRIORITY");

                entity.Property(e => e.RequestsRecovery)
                    .HasColumnType("BIT")
                    .HasColumnName("REQUESTS_RECOVERY");

                entity.Property(e => e.SchedTime)
                    .HasColumnType("BIGINT")
                    .HasColumnName("SCHED_TIME");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(16)")
                    .HasColumnName("STATE");

                entity.Property(e => e.TriggerGroup)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.TriggerName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_NAME");
            });

            modelBuilder.Entity<QrtzJobDetail>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.JobName, e.JobGroup });

                entity.ToTable("QRTZ_JOB_DETAILS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.JobName)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.JobGroup)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.Description)
                    .HasColumnType("NVARCHAR(250)")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.IsDurable)
                    .IsRequired()
                    .HasColumnType("BIT")
                    .HasColumnName("IS_DURABLE");

                entity.Property(e => e.IsNonconcurrent)
                    .IsRequired()
                    .HasColumnType("BIT")
                    .HasColumnName("IS_NONCONCURRENT");

                entity.Property(e => e.IsUpdateData)
                    .IsRequired()
                    .HasColumnType("BIT")
                    .HasColumnName("IS_UPDATE_DATA");

                entity.Property(e => e.JobClassName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(250)")
                    .HasColumnName("JOB_CLASS_NAME");

                entity.Property(e => e.JobData).HasColumnName("JOB_DATA");

                entity.Property(e => e.RequestsRecovery)
                    .IsRequired()
                    .HasColumnType("BIT")
                    .HasColumnName("REQUESTS_RECOVERY");
            });

            modelBuilder.Entity<QrtzLock>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.LockName });

                entity.ToTable("QRTZ_LOCKS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.LockName)
                    .HasColumnType("NVARCHAR(40)")
                    .HasColumnName("LOCK_NAME");
            });

            modelBuilder.Entity<QrtzPausedTriggerGrp>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerGroup });

                entity.ToTable("QRTZ_PAUSED_TRIGGER_GRPS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_GROUP");
            });

            modelBuilder.Entity<QrtzSchedulerState>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.InstanceName });

                entity.ToTable("QRTZ_SCHEDULER_STATE");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.InstanceName)
                    .HasColumnType("NVARCHAR(200)")
                    .HasColumnName("INSTANCE_NAME");

                entity.Property(e => e.CheckinInterval)
                    .HasColumnType("BIGINT")
                    .HasColumnName("CHECKIN_INTERVAL");

                entity.Property(e => e.LastCheckinTime)
                    .HasColumnType("BIGINT")
                    .HasColumnName("LAST_CHECKIN_TIME");
            });

            modelBuilder.Entity<QrtzSimpleTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_SIMPLE_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.RepeatCount)
                    .HasColumnType("BIGINT")
                    .HasColumnName("REPEAT_COUNT");

                entity.Property(e => e.RepeatInterval)
                    .HasColumnType("BIGINT")
                    .HasColumnName("REPEAT_INTERVAL");

                entity.Property(e => e.TimesTriggered)
                    .HasColumnType("BIGINT")
                    .HasColumnName("TIMES_TRIGGERED");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzSimpleTrigger)
                    .HasForeignKey<QrtzSimpleTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup });
            });

            modelBuilder.Entity<QrtzSimpropTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_SIMPROP_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR (120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasColumnType("NVARCHAR (150)")
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasColumnType("NVARCHAR (150)")
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.BoolProp1)
                    .HasColumnType("BIT")
                    .HasColumnName("BOOL_PROP_1");

                entity.Property(e => e.BoolProp2)
                    .HasColumnType("BIT")
                    .HasColumnName("BOOL_PROP_2");

                entity.Property(e => e.DecProp1)
                    .HasColumnType("NUMERIC")
                    .HasColumnName("DEC_PROP_1");

                entity.Property(e => e.DecProp2)
                    .HasColumnType("NUMERIC")
                    .HasColumnName("DEC_PROP_2");

                entity.Property(e => e.IntProp1)
                    .HasColumnType("INT")
                    .HasColumnName("INT_PROP_1");

                entity.Property(e => e.IntProp2)
                    .HasColumnType("INT")
                    .HasColumnName("INT_PROP_2");

                entity.Property(e => e.LongProp1)
                    .HasColumnType("BIGINT")
                    .HasColumnName("LONG_PROP_1");

                entity.Property(e => e.LongProp2)
                    .HasColumnType("BIGINT")
                    .HasColumnName("LONG_PROP_2");

                entity.Property(e => e.StrProp1)
                    .HasColumnType("NVARCHAR (512)")
                    .HasColumnName("STR_PROP_1");

                entity.Property(e => e.StrProp2)
                    .HasColumnType("NVARCHAR (512)")
                    .HasColumnName("STR_PROP_2");

                entity.Property(e => e.StrProp3)
                    .HasColumnType("NVARCHAR (512)")
                    .HasColumnName("STR_PROP_3");

                entity.Property(e => e.TimeZoneId)
                    .HasColumnType("NVARCHAR(80)")
                    .HasColumnName("TIME_ZONE_ID");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzSimpropTrigger)
                    .HasForeignKey<QrtzSimpropTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup });
            });

            modelBuilder.Entity<QrtzTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasColumnType("NVARCHAR(120)")
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.CalendarName)
                    .HasColumnType("NVARCHAR(200)")
                    .HasColumnName("CALENDAR_NAME");

                entity.Property(e => e.Description)
                    .HasColumnType("NVARCHAR(250)")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EndTime)
                    .HasColumnType("BIGINT")
                    .HasColumnName("END_TIME");

                entity.Property(e => e.JobData).HasColumnName("JOB_DATA");

                entity.Property(e => e.JobGroup)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(150)")
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.MisfireInstr).HasColumnName("MISFIRE_INSTR");

                entity.Property(e => e.NextFireTime)
                    .HasColumnType("BIGINT")
                    .HasColumnName("NEXT_FIRE_TIME");

                entity.Property(e => e.PrevFireTime)
                    .HasColumnType("BIGINT")
                    .HasColumnName("PREV_FIRE_TIME");

                entity.Property(e => e.Priority).HasColumnName("PRIORITY");

                entity.Property(e => e.StartTime)
                    .HasColumnType("BIGINT")
                    .HasColumnName("START_TIME");

                entity.Property(e => e.TriggerState)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(16)")
                    .HasColumnName("TRIGGER_STATE");

                entity.Property(e => e.TriggerType)
                    .IsRequired()
                    .HasColumnType("NVARCHAR(8)")
                    .HasColumnName("TRIGGER_TYPE");

                entity.HasOne(d => d.QrtzJobDetail)
                    .WithMany(p => p.QrtzTriggers)
                    .HasForeignKey(d => new { d.SchedName, d.JobName, d.JobGroup })
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

        }
    }
}
