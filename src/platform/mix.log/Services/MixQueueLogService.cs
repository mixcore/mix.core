using Microsoft.EntityFrameworkCore;
using Mix.Constant.Enums;
using Mix.Database.Entities.Queue;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Log.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Service.Services;
using Newtonsoft.Json.Linq;

namespace Mix.Log.Lib.Services
{
    public class MixQueueLogService : IMixQueueLog
    {
        private MixQueueDbContext _dbContext;
        private MixQueueMessages<MessageQueueModel> _mixQueueService;
        public int TenantId { get; set; }
        public MixQueueLogService()
        {
            _mixQueueService = new();
        }


        public async Task EnqueueMessageAsync(MessageQueueModel queueMessage)
        {
            try
            {
                using (_dbContext = new())
                {
                    InitDbContext();
                    if (queueMessage != null)
                    {
                        var queueLog = new MixQueueMessageLog()
                        {
                            Id = queueMessage.Id,
                            QueueMessageId = queueMessage.Id,
                            CreatedDateTime = DateTime.UtcNow,
                            Action = queueMessage.Action,
                            TopicId = queueMessage.TopicId,
                            DataTypeFullName = queueMessage.DataTypeFullName,
                            Subscriptions = ReflectionHelper.ParseArray(_mixQueueService.GetTopic(queueMessage.TopicId).Subscriptions),
                            State = MixQueueMessageLogState.NACK,
                            Status = MixContentStatus.Published
                        };
                        if (queueMessage.Data.IsJsonString())
                        {
                            queueLog.ObjectData = JObject.Parse(queueMessage.Data);
                        }
                        else
                        {
                            queueLog.StringData = queueMessage.Data;
                        }
                        _dbContext.MixQueueMessage.Add(queueLog);
                        await _dbContext.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public async Task AckQueueMessage(MessageQueueModel ackQueueMessage)
        {
            try
            {
                using (_dbContext = new())
                {
                    InitDbContext();

                    var rootLog = await _dbContext.MixQueueMessage.FirstOrDefaultAsync(m => m.Id == ackQueueMessage.Id);
                    if (rootLog != null)
                    {
                        var subs = rootLog.Subscriptions.FirstOrDefault(m => 
                            m.Value<string>("id") == ackQueueMessage.Sender) as JObject;
                        if (subs != null)
                        {
                            subs["status"] = MixQueueMessageLogState.ACK.ToString();
                            subs["lastModifed"] = DateTime.UtcNow;
                        }
                        if (!rootLog.Subscriptions.Any(m => m.Value<string>("status") != MixQueueMessageLogState.ACK.ToString()))
                        {
                            rootLog.State = MixQueueMessageLogState.ACK;
                        }
                        rootLog.LastModified = DateTime.UtcNow;
                        _dbContext.MixQueueMessage.Update(rootLog);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public async Task FailedQueueMessage(MessageQueueModel log)
        {
            try
            {
                using (_dbContext = new())
                {
                    InitDbContext();
                   
                    var rootLog = await _dbContext.MixQueueMessage.FirstOrDefaultAsync(m => m.Id == log.Id);
                    if (rootLog != null)
                    {
                        var subs = rootLog.Subscriptions.FirstOrDefault(m => m.Value<string>("id") == log.Sender) as JObject;
                        if (subs != null)
                        {
                            subs["status"] = MixQueueMessageLogState.FAILED.ToString();
                            subs["exception"] = ReflectionHelper.ParseObject(log.Exception);
                            subs["lastModifed"] = DateTime.UtcNow;
                            rootLog.State = MixQueueMessageLogState.FAILED;
                        }
                        rootLog.LastModified = DateTime.UtcNow;
                        _dbContext.MixQueueMessage.Update(rootLog);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        public async Task DeadLetterMessageAsync(MessageQueueModel deadLetterQueueMessage)
        {
            try
            {
                using (_dbContext = new())
                {
                    InitDbContext();

                    var rootLog = await _dbContext.MixQueueMessage.FirstOrDefaultAsync(m => m.Id == deadLetterQueueMessage.Id);
                    if (rootLog != null)
                    {
                        var subs = rootLog.Subscriptions.FirstOrDefault(m => m.Value<string>("id") == deadLetterQueueMessage.Sender);
                        if (subs != null)
                        {
                            subs["status"] = MixQueueMessageLogState.DEADLETTER.ToString();
                            subs["lastModifed"] = DateTime.UtcNow;
                        }
                        rootLog.State = MixQueueMessageLogState.DEADLETTER;
                        rootLog.LastModified = DateTime.UtcNow;
                        _dbContext.MixQueueMessage.Update(rootLog);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
            }
        }

        private void InitDbContext()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }
        }

    }
}
