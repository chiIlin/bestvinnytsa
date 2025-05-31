using Microsoft.EntityFrameworkCore;

namespace bestvinnytsa.web;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    // Зареєструйте тут свій DbSet<YourEntity>, коли створите сутність:
    // public DbSet<YourEntity> Entities { get; set; }
}