var builder = DistributedApplication.CreateBuilder(args);

var mixcore = builder.AddProject<Projects.mixcore>("mixcore");

builder.AddProject<Projects.mixcore_gateway>("mixcore.gateway")
            .WithReference(mixcore)
            ;

builder.AddProject<Projects.mix_mq>("mix.mq");

builder.Build().Run();
