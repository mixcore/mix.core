// ============================================
// Mixcore CMS
// Copyright (c) Mixcore Foundation. All rights reserved.
// Licensed under the GNU Affero General Public License v3.0 (AGPL-3.0).
// See LICENSE file in the project root for full license information.
// Commercial licenses available at https://mixcore.org/licensing
// ============================================

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
