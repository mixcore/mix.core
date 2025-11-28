using Mix.Database.Entities.Quartz;

namespace Mix.Database.EntityConfigurations.SQLSERVER.Quartz
{
    public class SqlServerQuartzConfigurations
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QrtzBlobTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_BLOB_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.BlobData).HasColumnName("BLOB_DATA");
            });

            modelBuilder.Entity<QrtzCalendar>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.CalendarName });

                entity.ToTable("QRTZ_CALENDARS");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.CalendarName)
                    .HasMaxLength(200)
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
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.CronExpression)
                    .IsRequired()
                    .HasMaxLength(120)
                    .HasColumnName("CRON_EXPRESSION");

                entity.Property(e => e.TimeZoneId)
                    .HasMaxLength(80)
                    .HasColumnName("TIME_ZONE_ID");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzCronTrigger)
                    .HasForeignKey<QrtzCronTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("FK_QRTZ_CRON_TRIGGERS_QRTZ_TRIGGERS");
            });

            modelBuilder.Entity<QrtzFiredTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.EntryId });

                entity.ToTable("QRTZ_FIRED_TRIGGERS");

                entity.HasIndex(e => new { e.SchedName, e.JobGroup, e.JobName }, "IDX_QRTZ_FT_G_J");

                entity.HasIndex(e => new { e.SchedName, e.TriggerGroup, e.TriggerName }, "IDX_QRTZ_FT_G_T");

                entity.HasIndex(e => new { e.SchedName, e.InstanceName, e.RequestsRecovery }, "IDX_QRTZ_FT_INST_JOB_REQ_RCVRY");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.EntryId)
                    .HasMaxLength(140)
                    .HasColumnName("ENTRY_ID");

                entity.Property(e => e.FiredTime).HasColumnName("FIRED_TIME");

                entity.Property(e => e.InstanceName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("INSTANCE_NAME");

                entity.Property(e => e.IsNonconcurrent).HasColumnName("IS_NONCONCURRENT");

                entity.Property(e => e.JobGroup)
                    .HasMaxLength(150)
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.JobName)
                    .HasMaxLength(150)
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.Priority).HasColumnName("PRIORITY");

                entity.Property(e => e.RequestsRecovery).HasColumnName("REQUESTS_RECOVERY");

                entity.Property(e => e.SchedTime).HasColumnName("SCHED_TIME");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("STATE");

                entity.Property(e => e.TriggerGroup)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.TriggerName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_NAME");
            });

            modelBuilder.Entity<QrtzJobDetail>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.JobName, e.JobGroup });

                entity.ToTable("QRTZ_JOB_DETAILS");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.JobName)
                    .HasMaxLength(150)
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.JobGroup)
                    .HasMaxLength(150)
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.IsDurable).HasColumnName("IS_DURABLE");

                entity.Property(e => e.IsNonconcurrent).HasColumnName("IS_NONCONCURRENT");

                entity.Property(e => e.IsUpdateData).HasColumnName("IS_UPDATE_DATA");

                entity.Property(e => e.JobClassName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("JOB_CLASS_NAME");

                entity.Property(e => e.JobData).HasColumnName("JOB_DATA");

                entity.Property(e => e.RequestsRecovery).HasColumnName("REQUESTS_RECOVERY");
            });

            modelBuilder.Entity<QrtzLock>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.LockName });

                entity.ToTable("QRTZ_LOCKS");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.LockName)
                    .HasMaxLength(40)
                    .HasColumnName("LOCK_NAME");
            });

            modelBuilder.Entity<QrtzPausedTriggerGrp>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerGroup });

                entity.ToTable("QRTZ_PAUSED_TRIGGER_GRPS");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_GROUP");
            });

            modelBuilder.Entity<QrtzSchedulerState>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.InstanceName });

                entity.ToTable("QRTZ_SCHEDULER_STATE");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.InstanceName)
                    .HasMaxLength(200)
                    .HasColumnName("INSTANCE_NAME");

                entity.Property(e => e.CheckinInterval).HasColumnName("CHECKIN_INTERVAL");

                entity.Property(e => e.LastCheckinTime).HasColumnName("LAST_CHECKIN_TIME");
            });

            modelBuilder.Entity<QrtzSimpleTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_SIMPLE_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.RepeatCount).HasColumnName("REPEAT_COUNT");

                entity.Property(e => e.RepeatInterval).HasColumnName("REPEAT_INTERVAL");

                entity.Property(e => e.TimesTriggered).HasColumnName("TIMES_TRIGGERED");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzSimpleTrigger)
                    .HasForeignKey<QrtzSimpleTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("FK_QRTZ_SIMPLE_TRIGGERS_QRTZ_TRIGGERS");
            });

            modelBuilder.Entity<QrtzSimpropTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_SIMPROP_TRIGGERS");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.BoolProp1).HasColumnName("BOOL_PROP_1");

                entity.Property(e => e.BoolProp2).HasColumnName("BOOL_PROP_2");

                entity.Property(e => e.DecProp1)
                    .HasColumnType("numeric(13, 4)")
                    .HasColumnName("DEC_PROP_1");

                entity.Property(e => e.DecProp2)
                    .HasColumnType("numeric(13, 4)")
                    .HasColumnName("DEC_PROP_2");

                entity.Property(e => e.IntProp1).HasColumnName("INT_PROP_1");

                entity.Property(e => e.IntProp2).HasColumnName("INT_PROP_2");

                entity.Property(e => e.LongProp1).HasColumnName("LONG_PROP_1");

                entity.Property(e => e.LongProp2).HasColumnName("LONG_PROP_2");

                entity.Property(e => e.StrProp1)
                    .HasMaxLength(512)
                    .HasColumnName("STR_PROP_1");

                entity.Property(e => e.StrProp2)
                    .HasMaxLength(512)
                    .HasColumnName("STR_PROP_2");

                entity.Property(e => e.StrProp3)
                    .HasMaxLength(512)
                    .HasColumnName("STR_PROP_3");

                entity.Property(e => e.TimeZoneId)
                    .HasMaxLength(80)
                    .HasColumnName("TIME_ZONE_ID");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzSimpropTrigger)
                    .HasForeignKey<QrtzSimpropTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("FK_QRTZ_SIMPROP_TRIGGERS_QRTZ_TRIGGERS");
            });

            modelBuilder.Entity<QrtzTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("QRTZ_TRIGGERS");

                entity.HasIndex(e => new { e.SchedName, e.CalendarName }, "IDX_QRTZ_T_C");

                entity.HasIndex(e => new { e.SchedName, e.JobGroup, e.JobName }, "IDX_QRTZ_T_G_J");

                entity.HasIndex(e => new { e.SchedName, e.NextFireTime }, "IDX_QRTZ_T_NEXT_FIRE_TIME");

                entity.HasIndex(e => new { e.SchedName, e.TriggerState, e.NextFireTime }, "IDX_QRTZ_T_NFT_ST");

                entity.HasIndex(e => new { e.SchedName, e.MisfireInstr, e.NextFireTime, e.TriggerState }, "IDX_QRTZ_T_NFT_ST_MISFIRE");

                entity.HasIndex(e => new { e.SchedName, e.MisfireInstr, e.NextFireTime, e.TriggerGroup, e.TriggerState }, "IDX_QRTZ_T_NFT_ST_MISFIRE_GRP");

                entity.HasIndex(e => new { e.SchedName, e.TriggerGroup, e.TriggerState }, "IDX_QRTZ_T_N_G_STATE");

                entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup, e.TriggerState }, "IDX_QRTZ_T_N_STATE");

                entity.HasIndex(e => new { e.SchedName, e.TriggerState }, "IDX_QRTZ_T_STATE");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(150)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.CalendarName)
                    .HasMaxLength(200)
                    .HasColumnName("CALENDAR_NAME");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EndTime).HasColumnName("END_TIME");

                entity.Property(e => e.JobData).HasColumnName("JOB_DATA");

                entity.Property(e => e.JobGroup)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.MisfireInstr).HasColumnName("MISFIRE_INSTR");

                entity.Property(e => e.NextFireTime).HasColumnName("NEXT_FIRE_TIME");

                entity.Property(e => e.PrevFireTime).HasColumnName("PREV_FIRE_TIME");

                entity.Property(e => e.Priority).HasColumnName("PRIORITY");

                entity.Property(e => e.StartTime).HasColumnName("START_TIME");

                entity.Property(e => e.TriggerState)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("TRIGGER_STATE");

                entity.Property(e => e.TriggerType)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("TRIGGER_TYPE");

                entity.HasOne(d => d.QrtzJobDetail)
                    .WithMany(p => p.QrtzTriggers)
                    .HasForeignKey(d => new { d.SchedName, d.JobName, d.JobGroup })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS");
            });
        }
    }
}
