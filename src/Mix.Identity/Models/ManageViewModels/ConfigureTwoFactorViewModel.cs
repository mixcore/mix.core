// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Mix.Identity.Models.ManageViewModels
{
    public class ConfigureTwoFactorViewModel
    {
        /// <summary>
        /// Gets or sets the selected provider.
        /// </summary>
        /// <value>
        /// The selected provider.
        /// </value>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets the providers.
        /// </summary>
        /// <value>
        /// The providers.
        /// </value>
        public ICollection<SelectListItem> Providers { get; set; }
    }
}