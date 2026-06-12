using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class RankRepository : IRankRepository
    {
        private TaskManagerDbContext _context;
        public RankRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public Rank Get(Rank item)
        {
            return _context.Ranks.Where(r => r.Id == item.Id).FirstOrDefault();
        }
        public List<Rank> GetAll()
        {
            return _context.Ranks.ToList();
        }
    }
}
