** Note: Cannot Use Mix Queue for microservices  because Mix using memory queue message. 

TODO: update Mix queue using Database or host to seperate microservice then it can be used in microservices architechture
- Example: TopicId = "uniqueTopic",
- Add Message Queue
```
private static IServiceCollection AddQueues(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
{

    // Message Queue
    services.AddSingleton<IQueueService<MessageQueueModel>, QueueService>();
    // Need singleton instance to store all message from mix publishers (inherit from MixPublisher)
    services.AddSingleton<MixMemoryMessageQueue<MessageQueueModel>>();
    return services;
}
```

- Push queue Message:
```
public class PostController {
    IQueueService<MessageQueueModel> _queueService;
    public PostController(IQueueService<MessageQueueModel> queueService)
    {
        _queueService = queueService;
    }
    [HttpPost]
    public async Task<ActionResult<TPrimaryKey>> Create([FromBody] PostModel post)
    {
        _queueService.PushQueue(new MessageQueueModel()
        {
            Action = "CreatePost",
            TopicId = "uniqueTopic",
            Model = JObject.FromObject(post),
            Status = MixRestStatus.Success
        });
    }
   
}
```
- Create Subscriber for SampleEntity
```
public class SampleSubscriber : SubscriberBase
{
    private UnitOfWorkInfo _uow;
    static string topicId = "uniqueTopic";
    public SampleSubscriber(
        IConfiguration configuration,
        MixMemoryMessageQueue<MessageQueueModel> queueService) 
        : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
    {
        _uow = new(new MixCmsContext());
    }

    public override async Task Handler(MessageQueueModel data)
    {        
        var post = data.ParseData<PostModel>();
        // Do something
        await _uow.CompleteAsync();
    }
}
```



- Add Subscriber Host Service
```
services.AddHostedService<SampleSubscriber>();
```


