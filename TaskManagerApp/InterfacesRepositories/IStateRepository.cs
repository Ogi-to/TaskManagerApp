using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IStateRepository
    {
        public Task<State> GetAsync(int stateId);

        public Task<List<State>> GetAllAsync();
    }
}
