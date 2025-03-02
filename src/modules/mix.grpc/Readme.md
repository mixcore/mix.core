# Every module use gRPC need to :
## 1. Install Grpc.AspNetCore (same version with this project)
## 2. Copy Protos to its source code
## 3. Rename namspace in proto file to current project namespace
## 4. Inlcue proto files in .csprj
```    
    <ItemGroup>
    <Protobuf Include="Domain\Protos\greet.proto" GrpcServices="Client" />
    </ItemGroup>
```
## 5. Check appsettings.json

```
    "Kestrel": {
        "EndpointDefaults": {
          "Protocols": "Http2"
        }
    }
```
## 6. Using MixGrpc

```
    @using Mix.Grpc.Models;
    var grpc = new GrpcClientModel<Greeter.GreeterClient>(endpointService.Mixcore);
    var reply = await grpc.Client.SayHelloAsync(
                        new HelloRequest { Name = "Greeter Client" });
```

# 7. Create a class inherit from MixGrpcService to handle message (ex: MixGrpcModuleService) then add to startup
```
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGrpcService<MixGrpcModuleService>();
    });
```

