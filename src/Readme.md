# Add new Module
## 1. Add module name in mix.shared/Constants/MixModuleNames
## 2. Add Endpoint in MixContent/AppConfigs/endpoint.json
## 3. Add Get/Set to MixEndpointService

# Run module as a microservice :
## 1. Remove Reference to module from Mixcore project
## 2. Copy MixContent Folder to Module's source code
## 3. Update ocelot.json.
(Ref: https://ocelot.readthedocs.io/en/latest/features/configuration.html). Ex:
```
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
    "UpstreamHttpMethod": [ "Get", "Post", "Put", "Patch", "Delete" ]
}
```
