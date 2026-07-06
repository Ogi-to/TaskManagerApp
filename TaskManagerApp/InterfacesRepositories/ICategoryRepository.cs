using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<Category> GetAsync(Category item);
        public Task<List<Category>> GetAllAsync();

    }
}
