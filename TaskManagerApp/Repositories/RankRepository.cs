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
        public async Task<Rank> GetAsync(int id)
        {
            return await _context.Ranks.Where(r => r.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Rank>> GetAllAsync()
        {
            return await _context.Ranks.ToListAsync();
        }

        public Task<Rank> GetRankForPoints(int points)
        {
            return _context.Ranks.Where(r => r.StartPoints <= points && r.EndPoints > points).FirstOrDefaultAsync();
        }
    }
}