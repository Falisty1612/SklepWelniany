using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SklepWelniany.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // pobieranie managerow
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Upewnienie istnienia ról
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Upewnienie istnienia konta Admin
            var adminEmail = "admin@localhost";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Tworzenie nowego admina
                adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                // WA¯NE: Jeœli siê nie uda, wyrzuæ b³¹d
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Nie uda³o siê utworzyæ konta Admina. B³êdy: {errors}");
                }
            }

            // Upewnienie czy admin ma role admina
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}