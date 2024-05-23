using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public class InvoiceCreateViewModel
    {
        public InvoiceCreateViewModel()
        {
            Items = new List<InvoiceItemViewModel>();
        }

        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Wybierz klienta z listy")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [DataType(DataType.Date)]
        [Display(Name = "Data wystawienia faktury")]
        public DateOnly IssueDate { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [DataType(DataType.Date)]
        [Display(Name = "Data sprzedaży")]
        public DateOnly SaleDate { get; set; }

        [Required(ErrorMessage = "Podaj kwotę całkowitą")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Dodaj pozycję faktury. Kwota całkowita musi być większa od 0.")]
        [Display(Name = "Kwota całkowita")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Uwagi")]
        [StringLength(255, ErrorMessage = "Pole nie może być dłuższe niż 255 znaków")]
        public string? Notes { get; set; }

        public List<InvoiceItemViewModel> Items { get; set; } = new List<InvoiceItemViewModel>();

        [Required(ErrorMessage = "Wybierz metodę płatności")]
        [Display(Name = "Metoda płatności")]
        public string PaymentMethod { get; set; } = string.Empty;

    }

    public class InvoiceItemViewModel
    {
        public int InvoiceItemId { get; set; }
        
        [Required(ErrorMessage = "Uzupełnij lub usuń cały wiersz")]
        [StringLength(55)]
        [Display(Name = "Opis")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Pole jest wymagane")]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa od 0")]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }
        
        [Required(ErrorMessage = "Pole jest wymagane")]
        [DataType(DataType.Currency, ErrorMessage = "Podaj cenę w formacie walutowym")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa od 0")]
        [Display(Name = "Cena")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Suma")]
        public decimal TotalPrice => Quantity * UnitPrice;
    }

}