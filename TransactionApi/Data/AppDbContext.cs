using Microsoft.EntityFrameworkCore;
using TransactionApi.Models;

namespace TransactionApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Username).IsUnique();
            });

            builder.Entity<Transaction>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasOne<User>().WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Payment>(b =>
            {
                b.HasKey(p => p.Id);
                b.HasIndex(p => p.Reference).IsUnique();
            });
        }
    }
}