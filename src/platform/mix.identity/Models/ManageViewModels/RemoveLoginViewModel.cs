// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Mix.Identity.Models.ManageViewModels
{
    public class RemoveLoginViewModel
    {
        /// <summary>
        /// Gets or sets the login provider.
        /// </summary>
        /// <value>
        /// The login provider.
        /// </value>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the provider key.
        /// </summary>
        /// <value>
        /// The provider key.
        /// </value>
        public string ProviderKey { get; set; }
    }
}