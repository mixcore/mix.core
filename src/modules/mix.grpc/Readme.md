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

```
    // The port number(5001) must match the port of the gRPC server.
    using var channel = GrpcChannel.ForAddress("https://localhost:5001");
    var client =  new Greeter.GreeterClient(channel);
    var reply = await client.SayHelloAsync(
                        new HelloRequest { Name = "GreeterClient" });
    Console.WriteLine("Greeting: " + reply.Message);
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
```

## 5. Check appsettings.json

```
    "Kestrel": {
        "EndpointDefaults": {
          "Protocols": "Http2"
        }
    }
```
