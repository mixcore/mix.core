# Google Cloud Pub/Sub :

- Enable Pub/Sub API https://console.cloud.google.com/apis/library/pubsub.googleapis.com
- Create Service Account https://console.cloud.google.com/iam-admin/serviceaccounts/create
- Grant Pub/Sub Admin
- Edit Service Account => create json key => save to MixContent/AppConfigs/google_credential.json
- https://console.cloud.google.com/cloudpubsub/topic/list?projec={projectId}
- Each Subscription have 1 subscriber only => to have multiple subscriber => create multi subscription

# Publish Mix Queue
## Solution 1:
- Create Publisher for model to create topic on google if not exist
```
 public class ThemePublisherService : GooglePublisherService<MixThemeViewModel>
{
    public ThemePublisherService(
        IQueueService<MessageQueueModel> queueService, 
        IConfiguration configuration, IWebHostEnvironment environment) 
        : base(queueService, configuration, environment)
    {
    }
}
```

```
public class PageContentPublisherService : PublisherServiceBase
{
    static string topicId = typeof(MixPageContentViewModel).FullName;
    public PageContentPublisherService(
        IQueueService<MessageQueueModel> queueService, 
        IConfiguration configuration, IWebHostEnvironment environment,
        MixMemoryMessageQueue<MessageQueueModel> queueMessage) 
        : base(topicId, queueService, configuration, environment, queueMessage)
    {
    }
}

```
*** TopicId: "uniqueTopic" (default fullname)

- Add Publisher Host Service
```
services.AddHostedService<ThemePublisherService>();
```
- Inject Mix Queue Message to push message
```
Inject IQueueService<MessageQueueModel> _queueService
```
- Push queue Message:
```
var post = new MixThemeViewModel()
{
    DisplayName = " test queue"
};
var msg = new MessageQueueModel();
msg.Package(post);
_queueService.PushQueue(msg);

// Or push custom message

_queueService.PushQueue(new MessageQueueModel()
{
    Action = Shared.Enums.MixRestAction.Get,
    TopicId = "uniqueTopic",
    Model = JObject.FromObject(products),
    Status = Shared.Enums.MixRestStatus.Success
});
```
## Solution 2:
- Use [GeneratePublisher] Attribute for ViewModel that need to generate Publisher

# Subscribe Queue
- Create Subscriber for each module
```
public class PageContentSubscriberService : SubscriberService
{
    protected UnitOfWorkInfo _uow;
    protected Repository<MixCmsContext, MixPageContent, int, MixPageContentViewModel> _repo;
    static string topicId = typeof(MixPageContentViewModel).FullName;
    public PageContentSubscriberService(
        IConfiguration configuration,
        MixMemoryMessageQueue<MessageQueueModel> queueService) : base(topicId, MixModuleNames.Portal, configuration, queueService)
    {
        _uow = new UnitOfWorkInfo(new MixCmsContext());
        _repo = MixPageContentViewModel.GetRepository(_uow);
    }

    public override Task Handler(MessageQueueModel data)
    {
        var post = data.Model.ToObject<MixPageContentViewModel>();
        switch (data.Action)
        {
            case MixRestAction.Get:
                break;
            case MixRestAction.Post:
                break;
            case MixRestAction.Put:
                break;
            case MixRestAction.Patch:
                break;
            case MixRestAction.Delete:
                break;
            default:
                break;
        }
        return Task.CompletedTask;
    }
}
```
- Add Subscriber Host Service
```
services.AddHostedService<ThemeSubscriberService>();
```
