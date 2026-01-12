using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SklepWelniany.Models
{
    public class SklepWelnianyDbContextFactory : IDesignTimeDbContextFactory<SklepWelnianyDbContext>
    {
        public SklepWelnianyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SklepWelnianyDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=SklepWelnianyDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new SklepWelnianyDbContext(optionsBuilder.Options);
        }
    }
}