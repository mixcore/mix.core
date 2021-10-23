# Google Cloud Pub/Sub :

- Enable Pub/Sub API https://console.cloud.google.com/apis/library/pubsub.googleapis.com
- Create Service Account https://console.cloud.google.com/iam-admin/serviceaccounts/create
- Edit Service Account => create json key => save to MixContent/AppConfigs/google_credential.json
- https://console.cloud.google.com/cloudpubsub/topic/list?projec={projectId}
- Each Subscription have 1 subscriber only => to have multiple subscriber => create multi subscription
# Publish Mix Queue
- Create Publisher for viewmodel to create topic on google if not exist
```
 public class ThemePublisherService : GooglePublisherService<MixThemeViewModel>
{
    public ThemePublisherService(
        IQueueService<QueueMessageModel> queueService, 
        IConfiguration configuration, IWebHostEnvironment environment) 
        : base(queueService, configuration, environment)
    {
    }
}
```
- Add Publisher Host Service
```
services.AddHostedService<ThemePublisherService>();
```
- Inject Mix Queue Message to push message
```
Inject IQueueService<QueueMessageModel> _queueService
```
- Push queue Message:
```
var post = new MixThemeViewModel()
{
    DisplayName = " test queue"
};
var msg = new QueueMessageModel();
msg.Package(post);
_queueService.PushQueue(msg);
```
# Subscribe Queue
- Create Subscriber for each module
```
public class ThemeSubscriberService : GoogleSubscriberService<MixThemeViewModel>
{
    public ThemeSubscriberService(
        IConfiguration configuration) : base("portal", configuration)
    {
    }

    public override Task Handler(QueueMessageModel data)
    {
        var post = data.Model.ToObject<MixThemeViewModel>();
        Console.WriteLine(post);
        return Task.CompletedTask;
    }
}
```
- Add Subscriber Host Service
```
ThemeSubscriberService
```
