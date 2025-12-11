// ============================================
// Mixcore CMS
// Copyright (c) Mixcore Foundation. All rights reserved.
// Licensed under the GNU Affero General Public License v3.0 (AGPL-3.0).
// See LICENSE file in the project root for full license information.
// Commercial licenses available at https://mixcore.org/licensing
// ============================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Mix.Scheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
