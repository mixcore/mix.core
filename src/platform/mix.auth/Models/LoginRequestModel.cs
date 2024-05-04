using System.ComponentModel.DataAnnotations;

namespace Mix.Auth.Models
{
    public sealed class LoginRequestModel

    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}[remember me].
        /// </summary>
        /// <value>
        /// {D255958A-8513-4226-94B9-080D98F904A1}  <c>true</c> if [remember me]; otherwise, <c>false</c>.
        /// </value>
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
