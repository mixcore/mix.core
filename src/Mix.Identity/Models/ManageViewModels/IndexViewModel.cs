// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Mix.Identity.Models.ManageViewModels
{
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has password.
        /// </summary>
        /// <value>
        /// {D255958A-8513-4226-94B9-080D98F904A1}  <c>true</c> if this instance has password; otherwise, <c>false</c>.
        /// </value>
        public bool HasPassword { get; set; }

        /// <summary>
        /// Gets or sets the logins.
        /// </summary>
        /// <value>
        /// The logins.
        /// </value>
        public IList<UserLoginInfo> Logins { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}[two factor].
        /// </summary>
        /// <value>
        /// {D255958A-8513-4226-94B9-080D98F904A1}  <c>true</c> if [two factor]; otherwise, <c>false</c>.
        /// </value>
        public bool TwoFactor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}[browser remembered].
        /// </summary>
        /// <value>
        /// {D255958A-8513-4226-94B9-080D98F904A1}  <c>true</c> if [browser remembered]; otherwise, <c>false</c>.
        /// </value>
        public bool BrowserRemembered { get; set; }
    }
}