using System;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Data
{
    public class BudgetTrackerDbContext : DbContext
    {
        public BudgetTrackerDbContext(DbContextOptions<BudgetTrackerDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expenditure> Expenditures { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<Income>(ConfigureIncome);
            modelBuilder.Entity<Expenditure>(ConfigureExpenditure);

        }
        // have exceeption about forigen key
        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
            builder.Property(u => u.HashedPassword).HasMaxLength(1024).IsRequired();
            builder.Property(u => u.FullName).HasMaxLength(50);
            builder.Property(u => u.JoinedOn).HasDefaultValueSql("getdate()");
            builder.Property(u => u.Salt).HasMaxLength(1024).IsRequired();
            builder.Property(u => u.FullName).HasMaxLength(50);
            builder.Ignore(u => u.TotalIncomes);
            builder.Ignore(u => u.TotalExpends);

        }
        private void ConfigureIncome(EntityTypeBuilder<Income> builder)
        {
            builder.ToTable("Income");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Amount).HasColumnType("money").IsRequired();
            builder.Property(i => i.Description).HasMaxLength(100);
            builder.Property(i => i.IncomeDate).HasDefaultValueSql("getdate()");
            builder.Property(i => i.Remarks).HasMaxLength(500);


        }
        private void ConfigureExpenditure(EntityTypeBuilder<Expenditure> builder)
        {
            builder.ToTable("Expenditure");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Amount).HasColumnType("money").IsRequired();
            builder.Property(e => e.Description).HasMaxLength(100);
            builder.Property(e => e.ExpDate).HasDefaultValueSql("getdate()");
            builder.Property(e => e.Remarks).HasMaxLength(500);
        }
    }
}
