using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _repository;
        public StateService(IStateRepository repository)
        {
            _repository = repository;

        }
        public async Task<List<State>> ShowAll()
        {
            var states = await _repository.GetAllAsync();
            if (states == null || states.Count == 0)
            {
                throw new Exception("There are no states.");
            }
            return states;
        }

        public async Task<State> ShowAsync(int stateId)
        {
            var existingState = await _repository.GetAsync(stateId);
            if (existingState == null)
            {
                throw new Exception($"State with ID {stateId} was not found.");
            }

            return existingState;
        }
    }
}
