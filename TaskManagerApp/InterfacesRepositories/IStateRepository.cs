using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IStateRepository
    {
        public Task<State> GetAsync(State item);

        public Task<List<State>> GetAllAsync();
    }
}
