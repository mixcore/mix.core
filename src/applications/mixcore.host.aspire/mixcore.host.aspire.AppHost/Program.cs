var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.mixcore>("mixcore");

builder.Build().Run();
