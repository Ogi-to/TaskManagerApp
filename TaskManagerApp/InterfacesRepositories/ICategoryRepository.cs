using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<Category> GetAsync(int categoryId);
        public Task<List<Category>> GetAllAsync();

    }
}
