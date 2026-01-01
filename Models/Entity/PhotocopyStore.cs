using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotocopyConnectedAPI.Models.Entity
{
    public class PhotocopyStore
    {
        [Key]
        public string StoreId { get; set; }

        [Required]
        [MaxLength(500)]
        public string StoreName { get; set; }

        [Required]
        [MaxLength(500)]
        public string StoreAddress { get; set; }

        [Required]
        [Phone]
        public string StorePhone { get; set; }

        [MaxLength(1000)]
        public string StoreDescription { get; set; }

        // Location for mapping
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // Business hours
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }

        // Status
        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        // Foreign Key: Store Owner (Chủ cửa hàng)
        [Required]
        public string OwnerId { get; set; }

        // Navigation Properties

        // 1-to-1: Store có 1 Owner
        public Account Owner { get; set; }

        // 1-to-many: Store có nhiều Employee (Nhân viên)
        public ICollection<Account> Employees { get; set; } = new HashSet<Account>();

        // 1-to-many: Store có nhiều Printer (Máy in)
        public ICollection<Printer> Printers { get; set; } = new HashSet<Printer>();

        // 1-to-many: Store nhận nhiều PhotocopyJob từ khách hàng
        public ICollection<PhotocopyJob> PhotocopyJobs { get; set; } = new HashSet<PhotocopyJob>();
    }
}