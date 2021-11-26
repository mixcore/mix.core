# Run module as a microservice :
## 1. Remove Reference to module from Mixcore project
## 2. Copy MixContent to Module's source code
## 3. Update ocelot.json.(Ref: https://ocelot.readthedocs.io/en/latest/features/configuration.html). 
Ex:
```
"Routes": [
    {
      "DownstreamPathTemplate": "/api/v2/rest/mix-account/{catchALl}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5002
        }
      ],
      "UpstreamPathTemplate": "/api/v2/rest/mix-account/{catchALl}",
      "UpstreamHttpMethod": [ "Get", "Post", "Put", "Patch", "Delete" ]
    },
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
]
```
## 4. Remove Config Http2 from appsettings if exist.
```
"Kestrel": {
    "EndpointDefaults": {
      "Protocols": "Http2"
    }
  }
```
## 5. Allow Cors.
## 6. Update "EnableOcelot" in mixcore -> global.json = true.
