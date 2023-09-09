using System.ComponentModel.DataAnnotations;

namespace IdentityTemplate.Identity.Models
{
    public class LoginInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LoginResponse
    {
        public string Pathname { get; set; }
        public LoginResponseState State { get; set; }
    }

    public class LoginResponseState
    {
        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
