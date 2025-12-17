using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SklepWelniany.Models
{
    public class SklepWelnianyDbContextFactory : IDesignTimeDbContextFactory<SklepWelnianyDbContext>
    {
        public SklepWelnianyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SklepWelnianyDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SklepWelnianyDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new SklepWelnianyDbContext(optionsBuilder.Options);
        }
    }
}
