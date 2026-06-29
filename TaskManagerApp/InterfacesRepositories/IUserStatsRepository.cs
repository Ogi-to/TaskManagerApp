using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserStatsRepository
    {
        public UserStats Get(UserStats item);

        public void Update(UserStats item);


    }
}
