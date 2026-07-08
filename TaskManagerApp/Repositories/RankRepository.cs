using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerApp.Repositories
{
    public class RankRepository : IRankRepository
    {
        private TaskManagerDbContext _context;
        public RankRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public async Task<Rank> GetAsync(int rankId)
        {
            return await _context.Ranks.Where(r => r.Id == rankId).FirstOrDefaultAsync();
        }
        public async Task<List<Rank>> GetAllAsync()
        {
            return await _context.Ranks.ToListAsync();
        }
    }
}
