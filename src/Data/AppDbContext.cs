using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public new DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Campaign>()
                .HasOne(ca => ca.Category)
                .WithMany(cat => cat.Campaigns)
                .HasForeignKey(ca => ca.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Campaign>()
                .HasKey(ca => ca.Id);

            modelBuilder.Entity<Campaign>()
                .Property(ca => ca.Name)
                .IsRequired();

            modelBuilder.Entity<Campaign>()
                .Property(ca => ca.Budget)
                .IsRequired();

            modelBuilder.Entity<Campaign>()
                .Property(ca => ca.Description)
                .IsRequired();

            modelBuilder.Entity<Campaign>()
                .Property(ca => ca.Link)
                .IsRequired();

            modelBuilder.Entity<Campaign>()
                .HasOne(ca => ca.Producer)
                .WithMany(u => u.Campaigns)
                .HasForeignKey(ca => ca.ProducerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Application>()
                .Property(a => a.ContactInfo)
                .IsRequired();

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Campaign)
                .WithMany(ca => ca.Applications)
                .HasForeignKey(a => a.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Influencer)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.InfluencerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.CampaignId, a.InfluencerId })
                .IsUnique();
        }
    }
}
