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

            // 1) Seeder для ролей
            string[] roles = new[] { "Producer", "Influencer" };
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

            // 2) Seeder для категорій
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
