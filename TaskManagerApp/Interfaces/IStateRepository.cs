using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IStateRepository
    {
        public State Get(State item);

        public void Update(State item);
    }
}
