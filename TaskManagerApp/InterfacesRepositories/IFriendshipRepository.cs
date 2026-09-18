using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Interfaces
{
    public interface IFriendshipRepository
    {
        // Изпращане на покана
        Task<Friendship> AddAsync(Friendship friendship);


        // Намиране на конкретно приятелство
        Task<Friendship?> GetAsync(int friendshipId);


        // Проверка дали вече съществува връзка
        Task<Friendship?> GetBetweenUsersAsync(
            int userId1,
            int userId2);


        // Получаване на всички приятелства на потребителя
        Task<List<Friendship>> GetUserFriendshipsAsync(
            int userId);


        // Получаване на входящите чакащи покани
        Task<List<Friendship>> GetPendingRequestsAsync(
            int userId);


        // Приемане / отказване
        Task UpdateAsync(Friendship friendship);


        // Изтриване на приятелство
        Task DeleteAsync(int friendshipId);
    }
}