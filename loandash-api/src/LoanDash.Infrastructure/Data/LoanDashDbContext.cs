/*
    * LoanDashDbContext.cs
    Describes the database schema and relationships.
    DbSet defines which classes map to what tables in the db
    OnModelCreating configures how those classes map to columns, defining constraints, relationships, etc.
*/

using LoanDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanDash.Infrastructure.Data;

public class LoanDashDbContext : DbContext
{
    public LoanDashDbContext(DbContextOptions<LoanDashDbContext> options): base(options){}

    public DbSet<Borrower> Borrowers => Set<Borrower>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Borrower
        modelBuilder.Entity<Borrower>(entity =>
        {
            entity.HasKey(b => b.BorrowerId);
            entity.Property(b => b.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(b => b.LastName).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Email).IsRequired().HasMaxLength(255);
        });

        // Configure Loan
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.LoanId);
            entity.Property(l => l.PrincipalAmount).HasPrecision(18, 2);
            entity.Property(l => l.InterestRate).HasPrecision(5, 4);
            entity.Property(l => l.OutstandingBalance).HasPrecision(18, 2);
            entity.Property(l => l.Status).HasConversion<int>(); // store enum as a int in the database 

            // Relationships
            entity.HasOne(l => l.Borrower)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BorrowerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId);
            entity.Property(p => p.AmountPaid).IsRequired().HasPrecision(18, 2);
            entity.Property(p => p.PrincipalPortion).IsRequired().HasPrecision(18, 2);
            entity.Property(p => p.InterestPortion).IsRequired().HasPrecision(18, 2);
            entity.Property(p => p.RemainingBalance).IsRequired().HasPrecision(18, 2);

            // Relationships
            entity.HasOne(p => p.Loan)
                .WithMany(l => l.Payments)
                .HasForeignKey(p => p.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}