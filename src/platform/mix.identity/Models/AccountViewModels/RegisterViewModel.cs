// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Mix.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.      
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public JObject Data { get; set; }
    }
}