using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopyConnectedAPI.Models.Entity
{
    public class PhotocopyJob
    {
        [Key]
        public string JobId { get; set; }

        // Foreign Keys
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string StoreId { get; set; }

        [Required]
        public string DocumentId { get; set; }

        public string? AssignedPrinterId { get; set; }

        public string ProcessedByEmployeeId { get; set; }

        // Print Settings
        [Required]
        public int NumberOfCopies { get; set; } = 1;

        [MaxLength(20)]
        public string PaperSize { get; set; } = "A4"; // A4, A3, Letter...

        // Status
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Cancelled

        // Pricing
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; } = 0;

        public bool IsPaid { get; set; } = false;

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? StartedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public Account Customer { get; set; }

        public PhotocopyStore Store { get; set; }

        public Document Document { get; set; }

        public Printer AssignedPrinter { get; set; }

        public Account ProcessedByEmployee { get; set; }
    }
}
