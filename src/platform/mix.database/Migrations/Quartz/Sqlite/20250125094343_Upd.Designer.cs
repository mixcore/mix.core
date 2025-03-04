﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Quartz;

#nullable disable

namespace Mix.Database.Migrations.Quartz.Sqlite
{
    [DbContext(typeof(SQLiteQuartzDbContext))]
    [Migration("20250125094343_Upd")]
    partial class Upd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzBlobTrigger", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<byte[]>("BlobData")
                        .HasColumnType("BLOB")
                        .HasColumnName("BLOB_DATA");

                    b.HasKey("SchedName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_BLOB_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzCalendar", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("CalendarName")
                        .HasColumnType("NVARCHAR(200)")
                        .HasColumnName("CALENDAR_NAME");

                    b.Property<byte[]>("Calendar")
                        .IsRequired()
                        .HasColumnType("BLOB")
                        .HasColumnName("CALENDAR");

                    b.HasKey("SchedName", "CalendarName");

                    b.ToTable("QRTZ_CALENDARS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzCronTrigger", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<string>("CronExpression")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("CRON_EXPRESSION");

                    b.Property<string>("TimeZoneId")
                        .HasColumnType("NVARCHAR(80)")
                        .HasColumnName("TIME_ZONE_ID");

                    b.HasKey("SchedName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_CRON_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzFiredTrigger", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("EntryId")
                        .HasColumnType("NVARCHAR(140)")
                        .HasColumnName("ENTRY_ID");

                    b.Property<long>("FiredTime")
                        .HasColumnType("BIGINT")
                        .HasColumnName("FIRED_TIME");

                    b.Property<string>("InstanceName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(200)")
                        .HasColumnName("INSTANCE_NAME");

                    b.Property<bool?>("IsNonconcurrent")
                        .HasColumnType("BIT")
                        .HasColumnName("IS_NONCONCURRENT");

                    b.Property<string>("JobGroup")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("JOB_GROUP");

                    b.Property<string>("JobName")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("JOB_NAME");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER")
                        .HasColumnName("PRIORITY");

                    b.Property<bool?>("RequestsRecovery")
                        .HasColumnType("BIT")
                        .HasColumnName("REQUESTS_RECOVERY");

                    b.Property<long>("SchedTime")
                        .HasColumnType("BIGINT")
                        .HasColumnName("SCHED_TIME");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(16)")
                        .HasColumnName("STATE");

                    b.Property<string>("TriggerGroup")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<string>("TriggerName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_NAME");

                    b.HasKey("SchedName", "EntryId");

                    b.ToTable("QRTZ_FIRED_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzJobDetail", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("JobName")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("JOB_NAME");

                    b.Property<string>("JobGroup")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("JOB_GROUP");

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<bool>("IsDurable")
                        .HasColumnType("BIT")
                        .HasColumnName("IS_DURABLE");

                    b.Property<bool>("IsNonconcurrent")
                        .HasColumnType("BIT")
                        .HasColumnName("IS_NONCONCURRENT");

                    b.Property<bool>("IsUpdateData")
                        .HasColumnType("BIT")
                        .HasColumnName("IS_UPDATE_DATA");

                    b.Property<string>("JobClassName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("JOB_CLASS_NAME");

                    b.Property<byte[]>("JobData")
                        .HasColumnType("BLOB")
                        .HasColumnName("JOB_DATA");

                    b.Property<bool>("RequestsRecovery")
                        .HasColumnType("BIT")
                        .HasColumnName("REQUESTS_RECOVERY");

                    b.HasKey("SchedName", "JobName", "JobGroup");

                    b.ToTable("QRTZ_JOB_DETAILS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzLock", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("LockName")
                        .HasColumnType("NVARCHAR(40)")
                        .HasColumnName("LOCK_NAME");

                    b.HasKey("SchedName", "LockName");

                    b.ToTable("QRTZ_LOCKS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzPausedTriggerGrp", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.HasKey("SchedName", "TriggerGroup");

                    b.ToTable("QRTZ_PAUSED_TRIGGER_GRPS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzSchedulerState", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("InstanceName")
                        .HasColumnType("NVARCHAR(200)")
                        .HasColumnName("INSTANCE_NAME");

                    b.Property<long>("CheckinInterval")
                        .HasColumnType("BIGINT")
                        .HasColumnName("CHECKIN_INTERVAL");

                    b.Property<long>("LastCheckinTime")
                        .HasColumnType("BIGINT")
                        .HasColumnName("LAST_CHECKIN_TIME");

                    b.HasKey("SchedName", "InstanceName");

                    b.ToTable("QRTZ_SCHEDULER_STATE", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzSimpleTrigger", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<int>("RepeatCount")
                        .HasColumnType("BIGINT")
                        .HasColumnName("REPEAT_COUNT");

                    b.Property<long>("RepeatInterval")
                        .HasColumnType("BIGINT")
                        .HasColumnName("REPEAT_INTERVAL");

                    b.Property<int>("TimesTriggered")
                        .HasColumnType("BIGINT")
                        .HasColumnName("TIMES_TRIGGERED");

                    b.HasKey("SchedName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_SIMPLE_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzSimpropTrigger", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR (120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("NVARCHAR (150)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("NVARCHAR (150)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<bool?>("BoolProp1")
                        .HasColumnType("BIT")
                        .HasColumnName("BOOL_PROP_1");

                    b.Property<bool?>("BoolProp2")
                        .HasColumnType("BIT")
                        .HasColumnName("BOOL_PROP_2");

                    b.Property<decimal?>("DecProp1")
                        .HasColumnType("NUMERIC")
                        .HasColumnName("DEC_PROP_1");

                    b.Property<decimal?>("DecProp2")
                        .HasColumnType("NUMERIC")
                        .HasColumnName("DEC_PROP_2");

                    b.Property<int?>("IntProp1")
                        .HasColumnType("INT")
                        .HasColumnName("INT_PROP_1");

                    b.Property<int?>("IntProp2")
                        .HasColumnType("INT")
                        .HasColumnName("INT_PROP_2");

                    b.Property<long?>("LongProp1")
                        .HasColumnType("BIGINT")
                        .HasColumnName("LONG_PROP_1");

                    b.Property<long?>("LongProp2")
                        .HasColumnType("BIGINT")
                        .HasColumnName("LONG_PROP_2");

                    b.Property<string>("StrProp1")
                        .HasColumnType("NVARCHAR (512)")
                        .HasColumnName("STR_PROP_1");

                    b.Property<string>("StrProp2")
                        .HasColumnType("NVARCHAR (512)")
                        .HasColumnName("STR_PROP_2");

                    b.Property<string>("StrProp3")
                        .HasColumnType("NVARCHAR (512)")
                        .HasColumnName("STR_PROP_3");

                    b.Property<string>("TimeZoneId")
                        .HasColumnType("NVARCHAR(80)")
                        .HasColumnName("TIME_ZONE_ID");

                    b.HasKey("SchedName", "TriggerName", "TriggerGroup");

                    b.ToTable("QRTZ_SIMPROP_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzTrigger", b =>
                {
                    b.Property<string>("SchedName")
                        .HasColumnType("NVARCHAR(120)")
                        .HasColumnName("SCHED_NAME");

                    b.Property<string>("TriggerName")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_NAME");

                    b.Property<string>("TriggerGroup")
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("TRIGGER_GROUP");

                    b.Property<string>("CalendarName")
                        .HasColumnType("NVARCHAR(200)")
                        .HasColumnName("CALENDAR_NAME");

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR(250)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<long?>("EndTime")
                        .HasColumnType("BIGINT")
                        .HasColumnName("END_TIME");

                    b.Property<byte[]>("JobData")
                        .HasColumnType("BLOB")
                        .HasColumnName("JOB_DATA");

                    b.Property<string>("JobGroup")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("JOB_GROUP");

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(150)")
                        .HasColumnName("JOB_NAME");

                    b.Property<int?>("MisfireInstr")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MISFIRE_INSTR");

                    b.Property<long?>("NextFireTime")
                        .HasColumnType("BIGINT")
                        .HasColumnName("NEXT_FIRE_TIME");

                    b.Property<long?>("PrevFireTime")
                        .HasColumnType("BIGINT")
                        .HasColumnName("PREV_FIRE_TIME");

                    b.Property<int?>("Priority")
                        .HasColumnType("INTEGER")
                        .HasColumnName("PRIORITY");

                    b.Property<long>("StartTime")
                        .HasColumnType("BIGINT")
                        .HasColumnName("START_TIME");

                    b.Property<string>("TriggerState")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(16)")
                        .HasColumnName("TRIGGER_STATE");

                    b.Property<string>("TriggerType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(8)")
                        .HasColumnName("TRIGGER_TYPE");

                    b.HasKey("SchedName", "TriggerName", "TriggerGroup");

                    b.HasIndex("SchedName", "JobName", "JobGroup");

                    b.ToTable("QRTZ_TRIGGERS", (string)null);
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzCronTrigger", b =>
                {
                    b.HasOne("Mix.Database.Entities.Quartz.QrtzTrigger", "QrtzTrigger")
                        .WithOne("QrtzCronTrigger")
                        .HasForeignKey("Mix.Database.Entities.Quartz.QrtzCronTrigger", "SchedName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QrtzTrigger");
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzSimpleTrigger", b =>
                {
                    b.HasOne("Mix.Database.Entities.Quartz.QrtzTrigger", "QrtzTrigger")
                        .WithOne("QrtzSimpleTrigger")
                        .HasForeignKey("Mix.Database.Entities.Quartz.QrtzSimpleTrigger", "SchedName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QrtzTrigger");
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzSimpropTrigger", b =>
                {
                    b.HasOne("Mix.Database.Entities.Quartz.QrtzTrigger", "QrtzTrigger")
                        .WithOne("QrtzSimpropTrigger")
                        .HasForeignKey("Mix.Database.Entities.Quartz.QrtzSimpropTrigger", "SchedName", "TriggerName", "TriggerGroup")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QrtzTrigger");
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzTrigger", b =>
                {
                    b.HasOne("Mix.Database.Entities.Quartz.QrtzJobDetail", "QrtzJobDetail")
                        .WithMany("QrtzTriggers")
                        .HasForeignKey("SchedName", "JobName", "JobGroup")
                        .IsRequired();

                    b.Navigation("QrtzJobDetail");
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzJobDetail", b =>
                {
                    b.Navigation("QrtzTriggers");
                });

            modelBuilder.Entity("Mix.Database.Entities.Quartz.QrtzTrigger", b =>
                {
                    b.Navigation("QrtzCronTrigger");

                    b.Navigation("QrtzSimpleTrigger");

                    b.Navigation("QrtzSimpropTrigger");
                });
#pragma warning restore 612, 618
        }
    }
}
