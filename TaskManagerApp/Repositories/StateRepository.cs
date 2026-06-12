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
        public State Get(State item)
        {
            return _context.States.Where(s => s.Id == item.Id).FirstOrDefault();
        }

        public List<State> GetAll()
        {
            return _context.States.ToList();
        }
    }
}
