using Mix.Database.Entities.Quartz;

namespace Mix.Database.EntityConfigurations.POSTGRES.Quartz
{
    public class PostgresSqlQuartzConfigurations
    {
        public static void Config(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QrtzBlobTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("qrtz_blob_triggers_pkey");

                entity.ToTable("qrtz_blob_triggers");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.TriggerName).HasColumnName("trigger_name");

                entity.Property(e => e.TriggerGroup).HasColumnName("trigger_group");

                entity.Property(e => e.BlobData).HasColumnName("blob_data");

            });

            modelBuilder.Entity<QrtzCalendar>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.CalendarName })
                    .HasName("qrtz_calendars_pkey");

                entity.ToTable("qrtz_calendars");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.CalendarName).HasColumnName("calendar_name");

                entity.Property(e => e.Calendar)
                    .IsRequired()
                    .HasColumnName("calendar");
            });

            modelBuilder.Entity<QrtzCronTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("qrtz_cron_triggers_pkey");

                entity.ToTable("qrtz_cron_triggers");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.TriggerName).HasColumnName("trigger_name");

                entity.Property(e => e.TriggerGroup).HasColumnName("trigger_group");

                entity.Property(e => e.CronExpression)
                    .IsRequired()
                    .HasColumnName("cron_expression");

                entity.Property(e => e.TimeZoneId).HasColumnName("time_zone_id");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzCronTrigger)
                    .HasForeignKey<QrtzCronTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("qrtz_cron_triggers_sched_name_trigger_name_trigger_group_fkey");
            });

            modelBuilder.Entity<QrtzFiredTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.EntryId })
                    .HasName("qrtz_fired_triggers_pkey");

                entity.ToTable("qrtz_fired_triggers");

                entity.HasIndex(e => e.JobGroup, "idx_qrtz_ft_job_group");

                entity.HasIndex(e => e.JobName, "idx_qrtz_ft_job_name");

                entity.HasIndex(e => e.RequestsRecovery, "idx_qrtz_ft_job_req_recovery");

                entity.HasIndex(e => e.TriggerGroup, "idx_qrtz_ft_trig_group");

                entity.HasIndex(e => e.InstanceName, "idx_qrtz_ft_trig_inst_name");

                entity.HasIndex(e => e.TriggerName, "idx_qrtz_ft_trig_name");

                entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }, "idx_qrtz_ft_trig_nm_gp");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.EntryId).HasColumnName("entry_id");

                entity.Property(e => e.FiredTime).HasColumnName("fired_time");

                entity.Property(e => e.InstanceName)
                    .IsRequired()
                    .HasColumnName("instance_name");

                entity.Property(e => e.IsNonconcurrent).HasColumnName("is_nonconcurrent");

                entity.Property(e => e.JobGroup).HasColumnName("job_group");

                entity.Property(e => e.JobName).HasColumnName("job_name");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.RequestsRecovery).HasColumnName("requests_recovery");

                entity.Property(e => e.SchedTime).HasColumnName("sched_time");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state");

                entity.Property(e => e.TriggerGroup)
                    .IsRequired()
                    .HasColumnName("trigger_group");

                entity.Property(e => e.TriggerName)
                    .IsRequired()
                    .HasColumnName("trigger_name");
            });

            modelBuilder.Entity<QrtzJobDetail>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.JobName, e.JobGroup })
                    .HasName("qrtz_job_details_pkey");

                entity.ToTable("qrtz_job_details");

                entity.HasIndex(e => e.RequestsRecovery, "idx_qrtz_j_req_recovery");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.JobName).HasColumnName("job_name");

                entity.Property(e => e.JobGroup).HasColumnName("job_group");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.IsDurable).HasColumnName("is_durable");

                entity.Property(e => e.IsNonconcurrent).HasColumnName("is_nonconcurrent");

                entity.Property(e => e.IsUpdateData).HasColumnName("is_update_data");

                entity.Property(e => e.JobClassName)
                    .IsRequired()
                    .HasColumnName("job_class_name");

                entity.Property(e => e.JobData).HasColumnName("job_data");

                entity.Property(e => e.RequestsRecovery).HasColumnName("requests_recovery");
            });

            modelBuilder.Entity<QrtzLock>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.LockName })
                    .HasName("qrtz_locks_pkey");

                entity.ToTable("qrtz_locks");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.LockName).HasColumnName("lock_name");
            });

            modelBuilder.Entity<QrtzPausedTriggerGrp>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerGroup })
                    .HasName("qrtz_paused_trigger_grps_pkey");

                entity.ToTable("qrtz_paused_trigger_grps");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.TriggerGroup).HasColumnName("trigger_group");
            });

            modelBuilder.Entity<QrtzSchedulerState>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.InstanceName })
                    .HasName("qrtz_scheduler_state_pkey");

                entity.ToTable("qrtz_scheduler_state");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.InstanceName).HasColumnName("instance_name");

                entity.Property(e => e.CheckinInterval).HasColumnName("checkin_interval");

                entity.Property(e => e.LastCheckinTime).HasColumnName("last_checkin_time");
            });

            modelBuilder.Entity<QrtzSimpleTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("qrtz_simple_triggers_pkey");

                entity.ToTable("qrtz_simple_triggers");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.TriggerName).HasColumnName("trigger_name");

                entity.Property(e => e.TriggerGroup).HasColumnName("trigger_group");

                entity.Property(e => e.RepeatCount).HasColumnName("repeat_count");

                entity.Property(e => e.RepeatInterval).HasColumnName("repeat_interval");

                entity.Property(e => e.TimesTriggered).HasColumnName("times_triggered");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzSimpleTrigger)
                    .HasForeignKey<QrtzSimpleTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("qrtz_simple_triggers_sched_name_trigger_name_trigger_group_fkey");
            });

            modelBuilder.Entity<QrtzSimpropTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("qrtz_simprop_triggers_pkey");

                entity.ToTable("qrtz_simprop_triggers");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.TriggerName).HasColumnName("trigger_name");

                entity.Property(e => e.TriggerGroup).HasColumnName("trigger_group");

                entity.Property(e => e.BoolProp1).HasColumnName("bool_prop_1");

                entity.Property(e => e.BoolProp2).HasColumnName("bool_prop_2");

                entity.Property(e => e.DecProp1).HasColumnName("dec_prop_1");

                entity.Property(e => e.DecProp2).HasColumnName("dec_prop_2");

                entity.Property(e => e.IntProp1).HasColumnName("int_prop_1");

                entity.Property(e => e.IntProp2).HasColumnName("int_prop_2");

                entity.Property(e => e.LongProp1).HasColumnName("long_prop_1");

                entity.Property(e => e.LongProp2).HasColumnName("long_prop_2");

                entity.Property(e => e.StrProp1).HasColumnName("str_prop_1");

                entity.Property(e => e.StrProp2).HasColumnName("str_prop_2");

                entity.Property(e => e.StrProp3).HasColumnName("str_prop_3");

                entity.Property(e => e.TimeZoneId).HasColumnName("time_zone_id");

                entity.HasOne(d => d.QrtzTrigger)
                    .WithOne(p => p.QrtzSimpropTrigger)
                    .HasForeignKey<QrtzSimpropTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("qrtz_simprop_triggers_sched_name_trigger_name_trigger_grou_fkey");
            });

            modelBuilder.Entity<QrtzTrigger>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                    .HasName("qrtz_triggers_pkey");

                entity.ToTable("qrtz_triggers");

                entity.HasIndex(e => e.NextFireTime, "idx_qrtz_t_next_fire_time");

                entity.HasIndex(e => new { e.NextFireTime, e.TriggerState }, "idx_qrtz_t_nft_st");

                entity.HasIndex(e => e.TriggerState, "idx_qrtz_t_state");

                entity.Property(e => e.SchedName).HasColumnName("sched_name");

                entity.Property(e => e.TriggerName).HasColumnName("trigger_name");

                entity.Property(e => e.TriggerGroup).HasColumnName("trigger_group");

                entity.Property(e => e.CalendarName).HasColumnName("calendar_name");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.JobData).HasColumnName("job_data");

                entity.Property(e => e.JobGroup)
                    .IsRequired()
                    .HasColumnName("job_group");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnName("job_name");

                entity.Property(e => e.MisfireInstr).HasColumnName("misfire_instr");

                entity.Property(e => e.NextFireTime).HasColumnName("next_fire_time");

                entity.Property(e => e.PrevFireTime).HasColumnName("prev_fire_time");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.TriggerState)
                    .IsRequired()
                    .HasColumnName("trigger_state");

                entity.Property(e => e.TriggerType)
                    .IsRequired()
                    .HasColumnName("trigger_type");

                entity.HasOne(d => d.QrtzJobDetail)
                    .WithMany(p => p.QrtzTriggers)
                    .HasForeignKey(d => new { d.SchedName, d.JobName, d.JobGroup })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("qrtz_triggers_sched_name_job_name_job_group_fkey");
            });

        }
    }
}
