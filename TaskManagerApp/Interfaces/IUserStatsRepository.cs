using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IUserStatsRepository
    {
        public UserStats Get(UserStats item);

        public UserStats Update(UserStats item);


    }
}
