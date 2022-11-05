// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Mix.Database.Services;

namespace Mix.Database.Entities.Account
{
    public partial class SqlServerAccountContext : MixCmsAccountContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MixCmsAccountContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public SqlServerAccountContext(DatabaseService databaseService)
                    : base(databaseService)
        {
        }
    }
}