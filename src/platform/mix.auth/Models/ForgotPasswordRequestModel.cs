using System.ComponentModel.DataAnnotations;

namespace Mix.Auth.Models
{
    public sealed class ForgotPasswordRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
