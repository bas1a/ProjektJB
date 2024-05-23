using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Podaj swój adres email.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
        [Display(Name = "Email")]
        public required string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj swoje hasło.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public required string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}