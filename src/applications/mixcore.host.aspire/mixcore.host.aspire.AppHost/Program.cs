var builder = DistributedApplication.CreateBuilder(args);

//var mixmq = builder.AddProject<Projects.mix_mq_server>("mix.mq.server");

var mixcore = builder.AddProject<Projects.mixcore>("mixcore");

//builder.AddProject<Projects.mix_auth_api>("mix.auth.api");
builder.AddProject<Projects.mixcore_gateway>("mixcore.gateway");

builder.Build().Run();
