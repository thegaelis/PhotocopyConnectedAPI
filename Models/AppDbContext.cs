using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotocopyConnectedAPI.Models.Entity;

public class ApplicationDbContext : IdentityDbContext<Account>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Account> Accounts { get; set; }
    public DbSet<PhotocopyStore> PhotocopyStores { get; set; }
    public DbSet<Printer> Printers { get; set; }
    public DbSet<PhotocopyJob> PhotocopyJobs { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ===== PhotocopyStore - Account (Owner) =====
        // 1 Owner có thể sở hữu nhiều Store
        builder.Entity<PhotocopyStore>()
            .HasOne(s => s.Owner)
            .WithMany(a => a.OwnedStores)
            .HasForeignKey(s => s.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== PhotocopyStore - Account (Employee) =====
        // 1 Store có nhiều Employee
        // 1 Employee chỉ thuộc 1 Store
        builder.Entity<Account>()
            .HasOne(a => a.EmployeeAtStore)
            .WithMany(s => s.Employees)
            .HasForeignKey(a => a.EmployeeAtStoreId)
            .OnDelete(DeleteBehavior.SetNull);

        // ===== PhotocopyStore - Printer =====
        // 1 Store có nhiều Printer
        builder.Entity<Printer>()
            .HasOne(p => p.Store)
            .WithMany(s => s.Printers)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== Account - Document =====
        // 1 Customer có thể upload nhiều Document
        builder.Entity<Document>()
            .HasOne(d => d.UploadedByUser)
            .WithMany(a => a.UploadedDocuments)
            .HasForeignKey(d => d.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== Document - PhotocopyJob =====
        // 1 Document có thể được in nhiều lần (nhiều PhotocopyJob)
        builder.Entity<PhotocopyJob>()
            .HasOne(j => j.Document)
            .WithMany(d => d.PhotocopyJobs)
            .HasForeignKey(j => j.DocumentId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== PhotocopyStore - PhotocopyJob =====
        // 1 Store nhận nhiều PhotocopyJob
        builder.Entity<PhotocopyJob>()
            .HasOne(j => j.Store)
            .WithMany(s => s.PhotocopyJobs)
            .HasForeignKey(j => j.StoreId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== Account (Customer) - PhotocopyJob =====
        // 1 Customer có thể tạo nhiều PhotocopyJob
        builder.Entity<PhotocopyJob>()
            .HasOne(j => j.Customer)
            .WithMany(a => a.PhotocopyJobs)
            .HasForeignKey(j => j.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // ===== Printer - PhotocopyJob =====
        // 1 Printer có thể xử lý nhiều PhotocopyJob
        builder.Entity<PhotocopyJob>()
            .HasOne(j => j.AssignedPrinter)
            .WithMany(p => p.PhotocopyJobs)
            .HasForeignKey(j => j.AssignedPrinterId)
            .OnDelete(DeleteBehavior.SetNull);

        // ===== Account (Employee) - PhotocopyJob (Processing) =====
        // 1 Employee có thể xử lý nhiều PhotocopyJob
        builder.Entity<PhotocopyJob>()
            .HasOne(j => j.ProcessedByEmployee)
            .WithMany(a => a.ProcessedJobs)
            .HasForeignKey(j => j.ProcessedByEmployeeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

