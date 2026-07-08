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
        public async Task<Category> GetAsync(int categoryId )
        {
            return await _context.Categories.Include(c => c.TasksCategories).ThenInclude(tc => tc.Task).FirstOrDefaultAsync(c => c.Id == categoryId);
        }
        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.TasksCategories).ThenInclude(tc => tc.Task).ToListAsync();
        }
    }
}
