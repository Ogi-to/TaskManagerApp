using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class StateRepository : IStateRepository
    {
        private TaskManagerDbContext _context;
        public StateRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public async Task<State> GetAsync(State item)
        {
            return await _context.States.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
        }

        public async Task<List<State>> GetAllAsync()
        {
            return await _context.States.ToListAsync();
        }
    }
}
