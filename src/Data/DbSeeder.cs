using System.Linq;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace bestvinnytsa.web.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndCategoriesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var context     = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            string[] roles = new[] { "Producer", "Influencer" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

         
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Косметика", Description = "Для косметичних брендів" },
                    new Category { Name = "Їжа", Description = "Товари харчування" },
                    new Category { Name = "Технології", Description = "Гаджети й електроніка" }
                );
                await context.SaveChangesAsync();
            }

       
            if (!context.Users.Any())
            {
                var prodUser = new AppUser
                {
                    UserName = "producer@test.com",
                    Email    = "producer@test.com",
                    FullName = "Тестовий Виробник",
                    RoleId   = 1,
                    IsEmailConfirmed = true
                };
                await userManager.CreateAsync(prodUser, "Password123!");
                await userManager.AddToRoleAsync(prodUser, "Producer");

                var infUser = new AppUser
                {
                    UserName = "influencer@test.com",
                    Email    = "influencer@test.com",
                    FullName = "Тестовий Інфлюенсер",
                    RoleId   = 2,
                    IsEmailConfirmed = true
                };
                await userManager.CreateAsync(infUser, "Password123!");
                await userManager.AddToRoleAsync(infUser, "Influencer");
            }
        }
    }
}
