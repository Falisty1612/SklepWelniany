using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SklepWelniany.Models;
using SklepWelniany.Data;

var builder = WebApplication.CreateBuilder(args);

//Connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//DbContext i Identity
builder.Services.AddDbContext<SklepWelnianyDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; //logowanie testowe
})
.AddRoles<IdentityRole>() // role: Admin / User
.AddEntityFrameworkStores<SklepWelnianyDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply migrations and seed roles/admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<SklepWelnianyDbContext>();
    await db.Database.MigrateAsync();
    await DbInitializer.InitializeAsync(services);
}

//Middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); //strona z b³êdami migracji
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); //Identity
app.UseAuthorization();

//Routing MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); //strony Identity: login, register

app.Run();
