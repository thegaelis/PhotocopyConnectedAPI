using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotocopyConnectedAPI.Models.Entity
{
    public class Printer
    {
        [Key]
        public string PrinterId { get; set; }

        [Required]
        [MaxLength(200)]
        public string PrinterName { get; set; }

        [MaxLength(100)]
        public string Model { get; set; }

        [MaxLength(100)]
        public string Brand { get; set; }

        // Printer capabilities/features
        [Required]
        public bool SupportsColor { get; set; } = false;

        [Required]
        public bool SupportsDuplexPrinting { get; set; } = false;

        // Status
        [Required]
        public bool IsOnline { get; set; } = true;

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime InstalledDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastMaintenanceDate { get; set; }

        // Foreign Key: Máy in thuộc về Store nào
        [Required]
        public int StoreId { get; set; }

        // Navigation Properties
        public PhotocopyStore Store { get; set; }

        // Một Printer có thể xử lý nhiều PhotocopyJob
        public ICollection<PhotocopyJob> PhotocopyJobs { get; set; } = new HashSet<PhotocopyJob>();
    }
}