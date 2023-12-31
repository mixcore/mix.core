var builder = DistributedApplication.CreateBuilder(args);

var mixcore = builder.AddProject<Projects.mixcore>("mixcore");

builder.AddProject<Projects.mixcore_gateway>("mixcore.gateway")
            .WithReference(mixcore)
            ;

builder.AddProject<Projects.mix_mq_server>("mix.mq.server");

builder.AddProject<Projects.mix_auth_service>("mix.auth.service");

builder.Build().Run();
