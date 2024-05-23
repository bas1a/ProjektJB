using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public class CustomerCreateViewModel
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Nazwa klienta jest wymagana")]
        [StringLength(100)]
        [Display(Name = "Nazwa klienta")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "NIP jest wymagany")]
        [StringLength(10)]
        [Display(Name = "NIP")]
        public string NIPNumber { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Miasto")]
        public string? City { get; set; }

        [StringLength(50)]
        [Display(Name = "Ulica")]
        public string? Street { get; set; }

        [StringLength(50)]
        [Display(Name = "Numer domu/mieszkania")]
        public string? Address { get; set; }

        [StringLength(10)]
        [Display(Name = "Kod pocztowy")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [StringLength(15)]
        [Display(Name = "Numer telefonu")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [StringLength(100)]
        [Display(Name = "Adres e-mail")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Niepoprawny adres e-mail")]
        public string Email { get; set; } = string.Empty;
    }
}