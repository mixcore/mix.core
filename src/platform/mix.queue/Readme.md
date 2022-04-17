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
public class TenantSubscriber : SubscriberBase
{
    private UnitOfWorkInfo _uow;
    static string topicId = typeof(MixTenantViewModel).FullName;
    public TenantSubscriber(
        IConfiguration configuration,
        MixMemoryMessageQueue<MessageQueueModel> queueService) 
        : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
    {
        _uow = new(new MixCmsContext());
    }

    public override async Task Handler(MessageQueueModel data)
    {
        var _repository = MixTenantViewModel.GetRepository(_uow);
        var post = data.Data.ToObject<MixTenantViewModel>();
        switch (data.Action)
        {
            case "Get":
                break;
            case "Post":
            case "Put":
            case "Patch":
            case "Delete":
                MixTenantRepository.Instance.AllTenants = await _repository.GetAllAsync(m => true);
                break;
            default:
                break;
        }
        await _uow.CompleteAsync();
    }
}
```
- Add Subscriber Host Service
```
services.AddHostedService<ThemeSubscriberService>();
```
