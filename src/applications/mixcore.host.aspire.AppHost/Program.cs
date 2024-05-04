var builder = DistributedApplication.CreateBuilder(args);

var mixMq = builder.AddProject<Projects.mix_mq_server>("mix-mq-server");

builder.AddProject<Projects.mixcore>("mixcore").WithReference(mixMq);
builder.AddProject<Projects.mixcore_gateway>("gateway").WithReference(mixMq);

builder.Build().Run();
