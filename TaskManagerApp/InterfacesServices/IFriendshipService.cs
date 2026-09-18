using TaskManagerApp.DTOS;

namespace TaskManagerApp.InterfacesServices
{
    public interface IFriendshipService
    {
        Task<FriendshipDto> SendFriendRequestAsync(
            int senderId,
            int receiverId);

        Task<List<FriendshipDto>> GetFriendsAsync(
            int userId);

        Task<List<FriendshipDto>> GetPendingRequestsAsync(
            int userId);

        Task AcceptFriendRequestAsync(
            int userId,
            int friendshipId);

        Task RejectFriendRequestAsync(
            int userId,
            int friendshipId);

        Task RemoveFriendAsync(
            int userId,
            int friendshipId);
    }
}