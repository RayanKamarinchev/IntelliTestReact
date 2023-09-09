using System.ComponentModel.DataAnnotations;

namespace IdentityTemplate.Identity.Models
{
    public class RegisterInput
    {
        [Required]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-]+)(\\.[a-zA-Z]{2,5}){1,2}$")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class RegisterResponse
    {
        public string Pathname { get; set; }
        public RegisterResponseState State { get; set; }
    }

    public class RegisterResponseState
    {
        public string ReturnUrl { get; set; }

        public string Email { get; set; }

        // Once you add a real email sender, you should remove this code that lets you confirm the account
        public string EmailConfirmationUrl { get; set; }
    }
}
