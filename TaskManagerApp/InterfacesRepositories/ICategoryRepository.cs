using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface ICategoryRepository
    {
        public Category Get(Category item);
        public List<Category> GetAll();

    }
}
