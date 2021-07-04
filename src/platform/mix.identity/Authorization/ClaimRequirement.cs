// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authorization;

namespace Mix.Identity.Authorization
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimRequirement" /> class.
        /// </summary>
        /// <param name="claimName">Name of the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        public ClaimRequirement(string claimName, string claimValue)
        {
            ClaimName = claimName;
            ClaimValue = claimValue;
        }

        /// <summary>
        /// Gets or sets the name of the claim.
        /// </summary>
        /// <value>
        /// The name of the claim.
        /// </value>
        public string ClaimName { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        /// <value>
        /// The claim value.
        /// </value>
        public string ClaimValue { get; set; }
    }
}