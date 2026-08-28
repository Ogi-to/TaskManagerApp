using TaskManagerApp.Data.Models;
using TaskManagerApp.Exceptions;
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
                throw new StateNotFoundException;
            }
            return states;
        }

        public async Task<State> ShowAsync(int stateId)
        {
            var existingState = await _repository.GetAsync(stateId);
            if (existingState == null)
            {
                throw new StateNotFoundException();
            }

            return existingState;
        }
    }
}
