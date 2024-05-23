using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj adres email.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Podaj numer telefonu.")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj imię.")]
        [Display(Name = "Imię")]
        [StringLength(30, ErrorMessage = "Imię musi mieć przynajmniej {2} i maksymalnie {1} znaków długości.", MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Podaj nazwisko.")]
        [Display(Name = "Nazwisko")]
        [StringLength(30, ErrorMessage = "Nazwisko musi mieć przynajmniej {2} i maksymalnie {1} znaków długości.", MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Wybierz rolę.")]
        [Display(Name = "Rola użytkownika")]
        public string UserRole { get; set; } = string.Empty;

        [Required(ErrorMessage = "Utwórz hasło.")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć przynajmniej {2} i maksymalnie {1} znaków długości.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potwierdź hasło.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasło i hasło potwierdzające nie zgadzają się.")]
        [Display(Name = "Potwierdź hasło")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}