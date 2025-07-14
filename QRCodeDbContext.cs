using Microsoft.EntityFrameworkCore;
using qr_website.Models;

namespace qr_website.Data
{
    public class QRCodeDbContext : DbContext
    {
        public QRCodeDbContext(DbContextOptions<QRCodeDbContext> options)
            : base(options)
        {
        }

        public DbSet<QRCodeEntry> QRCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QRCodeEntry>()
                .Property(q => q.Type)
                .HasConversion<int>();
        }
    }
}