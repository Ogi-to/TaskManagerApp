using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;

        public FriendshipService(
            IFriendshipRepository friendshipRepository,
            IUserRepository userRepository)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
        }


        // ============================================
        // SEND FRIEND REQUEST
        // ============================================

        public async Task<FriendshipDto>
            SendFriendRequestAsync(
                int senderId,
                int receiverId)
        {
            // Проверяваме изпращача
            var sender =
                await _userRepository.GetAsync(senderId);

            if (sender == null)
            {
                throw new UserNotFoundException(senderId);
            }


            // Проверяваме получателя
            var receiver =
                await _userRepository.GetAsync(receiverId);

            if (receiver == null)
            {
                throw new UserNotFoundException(receiverId);
            }


            // Не може да добавиш себе си
            if (senderId == receiverId)
            {
                throw new Exception(
                    "You cannot send a friend request to yourself.");
            }


            // Проверяваме дали вече има връзка
            var existing =
                await _friendshipRepository
                    .GetBetweenUsersAsync(
                        senderId,
                        receiverId);

            if (existing != null)
            {
                if (existing.Status ==
                    FriendshipStatus.Accepted)
                {
                    throw new Exception(
                        "You are already friends.");
                }

                if (existing.Status ==
                    FriendshipStatus.Pending)
                {
                    throw new Exception(
                        "A friend request already exists.");
                }

                // Ако старата покана е била отказана,
                // можем да създадем нова.
                await _friendshipRepository
                    .DeleteAsync(existing.Id);
            }


            var friendship = new Friendship
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = FriendshipStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };


            var created =
                await _friendshipRepository
                    .AddAsync(friendship);


            // Зареждаме потребителите,
            // за да можем да върнем DTO
            created.Sender = sender;
            created.Receiver = receiver;


            return MapToDto(created);
        }


        // ============================================
        // GET FRIENDS
        // ============================================

        public async Task<List<FriendshipDto>>
            GetFriendsAsync(int userId)
        {
            var user =
                await _userRepository.GetAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }


            var friendships =
                await _friendshipRepository
                    .GetUserFriendshipsAsync(userId);


            return friendships

                .Where(f =>
                    f.Status ==
                    FriendshipStatus.Accepted)

                .Select(MapToDto)

                .ToList();
        }


        // ============================================
        // GET PENDING REQUESTS
        // ============================================

        public async Task<List<FriendshipDto>>
            GetPendingRequestsAsync(
                int userId)
        {
            var user =
                await _userRepository.GetAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }


            var requests =
                await _friendshipRepository
                    .GetPendingRequestsAsync(userId);


            return requests

                .Select(MapToDto)

                .ToList();
        }


        // ============================================
        // ACCEPT FRIEND REQUEST
        // ============================================

        public async Task AcceptFriendRequestAsync(
            int userId,
            int friendshipId)
        {
            var friendship =
                await _friendshipRepository
                    .GetAsync(friendshipId);


            if (friendship == null)
            {
                throw new Exception(
                    "Friend request not found.");
            }


            // Само получателят може да приеме
            if (friendship.ReceiverId != userId)
            {
                throw new Exception(
                    "You cannot accept this friend request.");
            }


            if (friendship.Status !=
                FriendshipStatus.Pending)
            {
                throw new Exception(
                    "This friend request is no longer pending.");
            }


            friendship.Status =
                FriendshipStatus.Accepted;


            await _friendshipRepository
                .UpdateAsync(friendship);
        }


        // ============================================
        // REJECT FRIEND REQUEST
        // ============================================

        public async Task RejectFriendRequestAsync(
            int userId,
            int friendshipId)
        {
            var friendship =
                await _friendshipRepository
                    .GetAsync(friendshipId);


            if (friendship == null)
            {
                throw new Exception(
                    "Friend request not found.");
            }


            // Само получателят може да откаже
            if (friendship.ReceiverId != userId)
            {
                throw new Exception(
                    "You cannot reject this friend request.");
            }


            if (friendship.Status !=
                FriendshipStatus.Pending)
            {
                throw new Exception(
                    "This friend request is no longer pending.");
            }


            friendship.Status =
                FriendshipStatus.Rejected;


            await _friendshipRepository
                .UpdateAsync(friendship);
        }


        // ============================================
        // REMOVE FRIEND
        // ============================================

        public async Task RemoveFriendAsync(
            int userId,
            int friendshipId)
        {
            var friendship =
                await _friendshipRepository
                    .GetAsync(friendshipId);


            if (friendship == null)
            {
                throw new Exception(
                    "Friendship not found.");
            }


            // Трябва да си участник в приятелството
            if (friendship.SenderId != userId &&
                friendship.ReceiverId != userId)
            {
                throw new Exception(
                    "You cannot remove this friendship.");
            }


            // Само приети приятелства
            if (friendship.Status !=
                FriendshipStatus.Accepted)
            {
                throw new Exception(
                    "This friendship is not accepted.");
            }


            await _friendshipRepository
                .DeleteAsync(friendshipId);
        }


        // ============================================
        // MAP → DTO
        // ============================================

        private FriendshipDto MapToDto(
            Friendship friendship)
        {
            return new FriendshipDto
            {
                Id = friendship.Id,

                SenderId =
                    friendship.SenderId,

                SenderUsername =
                    friendship.Sender?.Username ?? "",

                ReceiverId =
                    friendship.ReceiverId,

                ReceiverUsername =
                    friendship.Receiver?.Username ?? "",

                Status =
                    friendship.Status.ToString(),

                CreatedAt =
                    friendship.CreatedAt
            };
        }
    }
}