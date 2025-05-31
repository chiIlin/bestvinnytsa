using Microsoft.EntityFrameworkCore;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Унікальні індекси
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Зв’язки: User → Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв’язки: Campaign → Producer (User)
            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.Producer)
                .WithMany(u => u.Campaigns)
                .HasForeignKey(c => c.ProducerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв’язки: Campaign → Category
            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Campaigns)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв’язки: Application → Campaign
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Campaign)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв’язки: Application → Influencer (User)
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Influencer)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.InfluencerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Унікальний індекс: один інфлюенсер не може подати дві заявки до тієї ж кампанії
            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.CampaignId, a.InfluencerId })
                .IsUnique();
        }
    }
}
