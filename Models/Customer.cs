using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(10)]
        public string NIPNumber { get; set; } = string.Empty;

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? Street { get; set; }

        [StringLength(50)]
        public string? Address { get; set; }

        [StringLength(10)]
        public string? PostalCode { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public ICollection<Invoice>? Invoices { get; set; }

    }
}