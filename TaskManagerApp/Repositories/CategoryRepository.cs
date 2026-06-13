using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private TaskManagerDbContext _context;
        public CategoryRepository(TaskManagerDbContext context)
        {
            _context = context;
        }
        public Category Get(Category item)
        {
            return _context.Categories.Where(c=> c.Id == item.Id).Include(c => c.Tasks).FirstOrDefault();
        }
        public List<Category> GetAll()
        {
            return _context.Categories.Include(c => c.Tasks).ToList();
        }
    }
}
