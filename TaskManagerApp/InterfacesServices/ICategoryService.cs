using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesServices
{
    public interface ICategoryService
    {
        public Task<Category> ShowAsync(int categoryId);
        public Task<List<Category>> ShowAll();
    }
}
