using System.Linq;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;
using bestvinnytsa.web.Data.Mongo;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace bestvinnytsa.web.Data
{
    /// <summary>
    /// Seeder для MongoDB: створює початкові ролі та категорії, якщо їх нема.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedRolesAndCategoriesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var mongoContext = scope.ServiceProvider.GetRequiredService<MongoContext>();

            var rolesCollection = mongoContext.Roles;
            var categoriesCollection = mongoContext.Categories;
            var usersCollection = mongoContext.Users;

            // 1) Seeder для ролей
            string[] roles = new[] { "Producer", "Influencer", "Admin" }; // ДОДАЛИ "Admin"
            foreach (var roleName in roles)
            {
                string normalizedRole = roleName.ToUpperInvariant();
                bool exists = await rolesCollection
                    .Find(r => r.NormalizedName == normalizedRole)
                    .AnyAsync();

                if (!exists)
                {
                    var newRole = new AppRole
                    {
                        Name = roleName,
                        NormalizedName = normalizedRole
                    };
                    await rolesCollection.InsertOneAsync(newRole);
                }
            }

            // 2) Створюємо адміністративний акаунт
            var adminEmail = "admin@admin";
            var existingAdmin = await usersCollection
                .Find(u => u.Email == adminEmail)
                .FirstOrDefaultAsync();

            if (existingAdmin == null)
            {
                var adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"), // пароль: admin
                    IsEmailConfirmed = true,
                    Roles = new List<string> { "Admin" },
                    FullName = "Системний адміністратор",
                    IsBlocked = false
                };

                await usersCollection.InsertOneAsync(adminUser);
                Console.WriteLine("Створено адміністративний акаунт: admin@admin / admin");
            }

            // 3) Seeder для категорій
            bool anyCategories = await categoriesCollection
                .Find(_ => true)
                .AnyAsync();

            if (!anyCategories)
            {
                await categoriesCollection.InsertManyAsync(new[]
                {
                    new Category { Name = "Косметика", Description = "Для косметичних брендів" },
                    new Category { Name = "Їжа", Description = "Товари харчування" },
                    new Category { Name = "Технології", Description = "Гаджети й електроніка" }
                });
            }
        }
    }
}
