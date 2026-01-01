using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PhotocopyConnectedAPI.Models.Entity
{
    public class Account : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        
        // JWT refresh token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Foreign Key: Nếu Account là Employee, chỉ thuộc 1 Store
        public string? EmployeeAtStoreId { get; set; }

        // Navigation Properties
        
        // As Store Owner: Một Owner có thể sở hữu nhiều Store
        public ICollection<PhotocopyStore> OwnedStores { get; set; } = new HashSet<PhotocopyStore>();
        
        // As Store Employee: Một Employee chỉ thuộc 1 Store
        public PhotocopyStore EmployeeAtStore { get; set; }
        
        // As Customer: Khách hàng có thể gửi nhiều PhotocopyJob
        public ICollection<PhotocopyJob> PhotocopyJobs { get; set; } = new HashSet<PhotocopyJob>();
        
        // As Employee Processing: Một Employee có thể xử lý nhiều PhotocopyJob
        public ICollection<PhotocopyJob> ProcessedJobs { get; set; } = new HashSet<PhotocopyJob>();
        
        // As Document Uploader: Khách hàng có thể upload nhiều Document
        public ICollection<Document> UploadedDocuments { get; set; } = new HashSet<Document>();
    }
}