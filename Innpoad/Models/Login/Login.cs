using System.ComponentModel.DataAnnotations;

namespace Innpoad.Models.Login
{
    public class Login
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool KeepLoggedIn { get; set; } = true;
    }
}
