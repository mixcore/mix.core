using Mix.Database.Entities.Quartz;

namespace Mix.Database.EntityConfigurations.MYSQL.Quartz
{
    public class MySqlQuartzConfigurations
    {
        public static void Config(ModelBuilder modelBuilder)
        {

            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<QrtzBlobTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("qrtz_blob_triggers");

                entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }, "SCHED_NAME");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.BlobData)
                    .HasColumnType("blob")
                    .HasColumnName("BLOB_DATA");
            });

            modelBuilder.Entity<QrtzCalendar>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.CalendarName })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("qrtz_calendars");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.CalendarName)
                    .HasMaxLength(200)
                    .HasColumnName("CALENDAR_NAME");

                entity.Property(e => e.Calendar)
                    .IsRequired()
                    .HasColumnType("blob")
                    .HasColumnName("CALENDAR");
            });

            modelBuilder.Entity<QrtzCronTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("qrtz_cron_triggers");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(200)
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("qrtz_cron_triggers_ibfk_1");
            });

            modelBuilder.Entity<QrtzFiredTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.EntryId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("qrtz_fired_triggers");

                entity.HasIndex(e => new { e.SchedName, e.InstanceName, e.RequestsRecovery }, "IDX_QRTZ_FT_INST_JOB_REQ_RCVRY");

                entity.HasIndex(e => new { e.SchedName, e.JobGroup }, "IDX_QRTZ_FT_JG");

                entity.HasIndex(e => new { e.SchedName, e.JobName, e.JobGroup }, "IDX_QRTZ_FT_J_G");

                entity.HasIndex(e => new { e.SchedName, e.TriggerGroup }, "IDX_QRTZ_FT_TG");

                entity.HasIndex(e => new { e.SchedName, e.InstanceName }, "IDX_QRTZ_FT_TRIG_INST_NAME");

                entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }, "IDX_QRTZ_FT_T_G");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.EntryId)
                    .HasMaxLength(140)
                    .HasColumnName("ENTRY_ID");

                entity.Property(e => e.FiredTime)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("FIRED_TIME");

                entity.Property(e => e.InstanceName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("INSTANCE_NAME");

                entity.Property(e => e.IsNonconcurrent).HasColumnName("IS_NONCONCURRENT");

                entity.Property(e => e.JobGroup)
                    .HasMaxLength(200)
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.JobName)
                    .HasMaxLength(200)
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.Priority)
                    .HasColumnType("int(11)")
                    .HasColumnName("PRIORITY");

                entity.Property(e => e.RequestsRecovery).HasColumnName("REQUESTS_RECOVERY");

                entity.Property(e => e.SchedTime)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("SCHED_TIME");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("STATE");

                entity.Property(e => e.TriggerGroup)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.TriggerName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_NAME");
            });

            modelBuilder.Entity<QrtzJobDetail>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.JobName, e.JobGroup })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("qrtz_job_details");

                entity.HasIndex(e => new { e.SchedName, e.JobGroup }, "IDX_QRTZ_J_GRP");

                entity.HasIndex(e => new { e.SchedName, e.RequestsRecovery }, "IDX_QRTZ_J_REQ_RECOVERY");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.JobName)
                    .HasMaxLength(200)
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.JobGroup)
                    .HasMaxLength(200)
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

                entity.Property(e => e.JobData)
                    .HasColumnType("blob")
                    .HasColumnName("JOB_DATA");

                entity.Property(e => e.RequestsRecovery).HasColumnName("REQUESTS_RECOVERY");
            });

            modelBuilder.Entity<QrtzLock>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.LockName })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("qrtz_locks");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.LockName)
                    .HasMaxLength(40)
                    .HasColumnName("LOCK_NAME");
            });

            modelBuilder.Entity<QrtzPausedTriggerGrp>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerGroup })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("qrtz_paused_trigger_grps");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_GROUP");
            });

            modelBuilder.Entity<QrtzSchedulerState>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.InstanceName })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("qrtz_scheduler_state");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.InstanceName)
                    .HasMaxLength(200)
                    .HasColumnName("INSTANCE_NAME");

                entity.Property(e => e.CheckinInterval)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("CHECKIN_INTERVAL");

                entity.Property(e => e.LastCheckinTime)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("LAST_CHECKIN_TIME");
            });

            modelBuilder.Entity<QrtzSimpleTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("qrtz_simple_triggers");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.RepeatCount)
                    .HasColumnType("bigint(7)")
                    .HasColumnName("REPEAT_COUNT");

                entity.Property(e => e.RepeatInterval)
                    .HasColumnType("bigint(12)")
                    .HasColumnName("REPEAT_INTERVAL");

                entity.Property(e => e.TimesTriggered)
                    .HasColumnType("bigint(10)")
                    .HasColumnName("TIMES_TRIGGERED");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzSimpleTrigger)
                    .HasForeignKey<QrtzSimpleTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("qrtz_simple_triggers_ibfk_1");
            });

            modelBuilder.Entity<QrtzSimpropTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("qrtz_simprop_triggers");

                entity.Property(e => e.SchedName)
                    .HasMaxLength(120)
                    .HasColumnName("SCHED_NAME");

                entity.Property(e => e.TriggerName)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.BoolProp1).HasColumnName("BOOL_PROP_1");

                entity.Property(e => e.BoolProp2).HasColumnName("BOOL_PROP_2");

                entity.Property(e => e.DecProp1)
                    .HasPrecision(13, 4)
                    .HasColumnName("DEC_PROP_1");

                entity.Property(e => e.DecProp2)
                    .HasPrecision(13, 4)
                    .HasColumnName("DEC_PROP_2");

                entity.Property(e => e.IntProp1)
                    .HasColumnType("int(11)")
                    .HasColumnName("INT_PROP_1");

                entity.Property(e => e.IntProp2)
                    .HasColumnType("int(11)")
                    .HasColumnName("INT_PROP_2");

                entity.Property(e => e.LongProp1)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("LONG_PROP_1");

                entity.Property(e => e.LongProp2)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("LONG_PROP_2");

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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("qrtz_simprop_triggers_ibfk_1");
            });

            modelBuilder.Entity<QrtzTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("qrtz_triggers");

                entity.HasIndex(e => new { e.SchedName, e.CalendarName }, "IDX_QRTZ_T_C");

                entity.HasIndex(e => new { e.SchedName, e.TriggerGroup }, "IDX_QRTZ_T_G");

                entity.HasIndex(e => new { e.SchedName, e.JobName, e.JobGroup }, "IDX_QRTZ_T_J");

                entity.HasIndex(e => new { e.SchedName, e.JobGroup }, "IDX_QRTZ_T_JG");

                entity.HasIndex(e => new { e.SchedName, e.NextFireTime }, "IDX_QRTZ_T_NEXT_FIRE_TIME");

                entity.HasIndex(e => new { e.SchedName, e.MisfireInstr, e.NextFireTime }, "IDX_QRTZ_T_NFT_MISFIRE");

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
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_NAME");

                entity.Property(e => e.TriggerGroup)
                    .HasMaxLength(200)
                    .HasColumnName("TRIGGER_GROUP");

                entity.Property(e => e.CalendarName)
                    .HasMaxLength(200)
                    .HasColumnName("CALENDAR_NAME");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EndTime)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("END_TIME");

                entity.Property(e => e.JobData)
                    .HasColumnType("blob")
                    .HasColumnName("JOB_DATA");

                entity.Property(e => e.JobGroup)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("JOB_GROUP");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("JOB_NAME");

                entity.Property(e => e.MisfireInstr)
                    .HasColumnType("smallint(2)")
                    .HasColumnName("MISFIRE_INSTR");

                entity.Property(e => e.NextFireTime)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("NEXT_FIRE_TIME");

                entity.Property(e => e.PrevFireTime)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("PREV_FIRE_TIME");

                entity.Property(e => e.Priority)
                    .HasColumnType("int(11)")
                    .HasColumnName("PRIORITY");

                entity.Property(e => e.StartTime)
                    .HasColumnType("bigint(19)")
                    .HasColumnName("START_TIME");

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
                    .HasConstraintName("qrtz_triggers_ibfk_1");
            });

        }
    }
}
