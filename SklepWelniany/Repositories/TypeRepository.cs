using Microsoft.EntityFrameworkCore;
using SklepWelniany.Models;

namespace SklepWelniany.Repositories
{
    public interface ITypeRepository
    {
        Task AddType(Models.Type type);
        Task UpdateType(Models.Type type);
        Task<Models.Type?> GetTypeById(int id);
        Task DeleteType(Models.Type type);
        Task<IEnumerable<Models.Type>> GetTypes();
    }
    public class TypeRepository : ITypeRepository
    {
        private readonly ApplicationDbContext _context;
        public TypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddType(Models.Type type)
        {
            _context.Types.Add(type);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateType(Models.Type type)
        {
            _context.Types.Update(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteType(Models.Type type)
        {
            _context.Types.Remove(type);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Type?> GetTypeById(int id)
        {
            return await _context.Types.FindAsync(id);
        }

        public async Task<IEnumerable<Models.Type>> GetTypes()
        {
            return await _context.Types.ToListAsync();
        }


    }
}