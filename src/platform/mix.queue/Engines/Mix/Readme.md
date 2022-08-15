** Note: Cannot Use Mix Queue for microservices  because Mix using memory queue message. 
TODO: update Mix queue using Database or host to seperate microservice then it can be used in microservices architechture

- Create Publisher for model to create topic on google if not exist
```
public class PageContentPublisherService : PublisherServiceBase
{
    static string topicId = typeof(SampleEntity).FullName;
    public PageContentPublisherService(
        IQueueService<MessageQueueModel> queueService, 
        IConfiguration configuration, IWebHostEnvironment environment,
        MixMemoryMessageQueue<MessageQueueModel> queueMessage) 
        : base(topicId, queueService, configuration, environment, queueMessage)
    {
    }
}

```
- Create Subscriber for each module
```
public class TenantSubscriber : SubscriberBase
{
    private UnitOfWorkInfo _uow;
    static string topicId = typeof(SampleEntity).FullName;
    public TenantSubscriber(
        IConfiguration configuration,
        MixMemoryMessageQueue<MessageQueueModel> queueService) 
        : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
    {
        _uow = new(new MixCmsContext());
    }

    public override async Task Handler(MessageQueueModel data)
    {
        // Do something
        await _uow.CompleteAsync();
    }
}
```
- Add Publisher Host Service
```
services.AddHostedService<ThemePublisherService>();
```
- Add Subscriber Host Service
```
services.AddHostedService<ThemeSubscriberService>();
```

- Inject Mix Queue Message to push message
```
Inject IQueueService<MessageQueueModel> _queueService
```
- Push queue Message:
```
var post = new SampleEntity()
{
    DisplayName = " test queue"
};
var msg = new MessageQueueModel();
msg.Package(post);
_queueService.PushQueue(msg);
```
- Or push custom message
```
_queueService.PushQueue(new MessageQueueModel()
{
    Action = Shared.Enums.MixRestAction.Get,
    TopicId = "uniqueTopic",
    Model = JObject.FromObject(products),
    Status = Shared.Enums.MixRestStatus.Success
});
```