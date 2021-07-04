// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Mix.Identity.Models.ManageViewModels
{
    public class ManageLoginsViewModel
    {
        /// <summary>
        /// Gets or sets the current logins.
        /// </summary>
        /// <value>
        /// The current logins.
        /// </value>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        /// Gets or sets the other logins.
        /// </summary>
        /// <value>
        /// The other logins.
        /// </value>
        public IList<AuthenticationScheme> OtherLogins { get; set; }
    }
}