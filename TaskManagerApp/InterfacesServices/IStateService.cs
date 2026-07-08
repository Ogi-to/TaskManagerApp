using TaskManagerApp.Data.Models;

namespace TaskManagerApp.InterfacesServices
{
    public interface IStateService
    {
        public Task<State> ShowAsync(int stateId);
        public Task<List<State>> ShowAll();
    }
}
