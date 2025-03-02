*Notes:*
### Ref: https://learn.microsoft.com/en-us/dotnet/core/runtime-config/garbage-collector#affinitize
# Run module as a microservice :
## 1. Remove Reference to module from Mixcore project
## 2. Copy MixContent to Module's source code
## 3. Update ocelot.json.(Ref: https://ocelot.readthedocs.io/en/latest/features/configuration.html). 
Ex:
```
git reset HEAD src/platform/core/mix-heart

{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/v2/rest/mix-portal/{catchALl}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5006
        }
      ],
      "UpstreamPathTemplate": "/api/v2/rest/mix-portal/{catchALl}",
      "UpstreamHttpMethod": [ "Get", "Post", "Put", "Patch", "Delete" ],
      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "1s",
        "PeriodTimespan": 1,
        "Limit": 1000
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:5010"
  }  
}
  
```
## 4. Remove Config Http2 from appsettings if exist.
```
"Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http2"
    }
  }
```
## 5. Update "EnableOcelot" in mixcore -> global.json = true.

**Make sure the enpoints in MixContent/AppConfigs/endpoints.json are correct (when deploy to from local to production, must change the localhost to real endpoints)**
## 5. Update Enpoints to production domain when golive (/MixContent/AppConfigs/enpoints.json)

## 6.Add migration
** Move to mix.database folder
** dotnet ef --startup-project ../../applications/Mixcore migrations add Init --context PostgresqlmixcmsContext --output-dir Migrations/Cms/PostgresqlMixCms