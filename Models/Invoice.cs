using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingProgram.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        public DateOnly IssueDate { get; set; }

        public DateOnly SaleDate { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public string Notes { get; set; } = string.Empty;

        public ICollection<InvoiceItem>? InvoiceItems { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;



        // Relacja z tabelą Users
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        
        
        // Relacja z tabelą Customers
        public int? CustomerId { get; set; }
    
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}