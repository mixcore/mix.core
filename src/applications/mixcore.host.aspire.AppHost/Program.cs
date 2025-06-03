var builder = DistributedApplication.CreateBuilder(args);

var mixcore = builder.AddProject<Projects.mixcore>("mixcore");
//builder.AddProject<Projects.mixcore_gateway>("mixcore-gateway").WithReference(mixcore);

//builder.AddProject<Projects.mix_automation_api>("mix-automation-api");

//builder.AddProject<Projects.mix_mqtt>("mix-mqtt");
//builder.AddProject<Projects.mixcore_gateway>("mixcore-gateway").WithReference(mixcore);

//builder.AddProject<Projects.mix_automation_api>("mix-automation-api");

//builder.AddProject<Projects.mix_mcp_api>("mix-mcp-api");
//builder.AddProject<Projects.mixcore_gateway>("mixcore-gateway").WithReference(mixcore);

//builder.AddProject<Projects.mix_automation_api>("mix-automation-api");

//builder.AddProject<Projects.mix_mqtt>("mix-mqtt");
//builder.AddProject<Projects.mixcore_gateway>("mixcore-gateway").WithReference(mixcore);

//builder.AddProject<Projects.mix_automation_api>("mix-automation-api");

//builder.AddProject<Projects.mixcore_spa_Server>("mixcore-spa-server");

builder.Build().Run();
