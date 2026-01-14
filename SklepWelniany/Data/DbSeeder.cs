using Microsoft.AspNetCore.Identity;
using SklepWelniany.Constants;

namespace SklepWelniany.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            var context = service.GetService<ApplicationDbContext>();

            // Dodawanie Ról
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // Tworzenie Admina
            var admin = new IdentityUser
            {
                UserName = "admin@localhost.com",
                Email = "admin@localhost.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@99");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }

            // Kategorie
            var typesToAdd = new List<string>
            {
                "Kardigan",
                "Sweter",
                "Czapka",
                "Szalik",
                // DODANIE KATEGORII / DODANIE TYPU
            };

            foreach (var typeName in typesToAdd)
            {
                // sprawdzenie czy kategoria juz istnieje
                var exists = await context.Types
                    .AnyAsync(t => t.ProductType.ToLower() == typeName.ToLower());

                if (!exists)
                {
                    await context.Types.AddAsync(new SklepWelniany.Models.Type
                    {
                        ProductType = typeName
                    });
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
