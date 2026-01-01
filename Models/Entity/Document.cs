using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhotocopyConnectedAPI.Const;

namespace PhotocopyConnectedAPI.Models.Entity
{
    public class Document
    {
        [Key]
        public string DocumentId { get; set; }

        [Required]
        [MaxLength(200)]
        public string DocumentTitle { get; set; }

        [Required]
        [MaxLength(500)]
        public string DocumentFilePath { get; set; }

        [Required]
        public int NumberOfPages { get; set; }

        [MaxLength(10)]
        public string FileExtension { get; set; } // .pdf, .docx, .xlsx...

        [MaxLength(50)]
        public string FileSize { get; set; } // "2.5 MB"

        // Document Type - 3 loại tài liệu
        [Required]
        public DocumentType DocumentType { get; set; } = DocumentType.Single;

        // Custom note cho loại Custom document
        [MaxLength(1000)]
        public string CustomNote { get; set; }

        // Timestamps
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsActive { get; set; } = true;

        // Foreign Key: Tài liệu được upload bởi khách hàng nào
        [Required]
        public string UploadedByUserId { get; set; }

        // Navigation Properties
        public Account UploadedByUser { get; set; }

        // Một Document có thể được in nhiều lần (nhiều PhotocopyJob)
        public ICollection<PhotocopyJob> PhotocopyJobs { get; set; } = new HashSet<PhotocopyJob>();
    }
}