// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account
{
    public partial class PostgresSqlAccountContext : MixCmsAccountContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MixCmsAccountContext" /> class.
        /// </summary>
        /// <param name="databaseService"></param>
        public PostgresSqlAccountContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}