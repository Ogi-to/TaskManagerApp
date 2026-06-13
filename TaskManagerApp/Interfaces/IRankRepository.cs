using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IRankRepository
    {
        public Rank Get(Rank item);
        public List<Rank> GetAll();

    }
}
