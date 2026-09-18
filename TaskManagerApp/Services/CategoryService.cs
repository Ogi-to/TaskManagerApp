using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;

        }
        public async Task<List<Category>> ShowAll()
        {
            var categories = await _repository.GetAllAsync();
            if (categories == null || categories.Count == 0)
            {
                throw new CategoryNotFoundException();
            }
            return categories;
        }

        public async Task<Category> ShowAsync(int categoryId)
        {
            var existingCategory = await _repository.GetAsync(categoryId);
            if (existingCategory == null)
            {
                throw new CategoryNotFoundException();
            }

            return existingCategory;
        } 
    }
}
