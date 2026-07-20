using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.DtoMappers
{

    public static class UserMapper
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                RankId = user.RankId,
                Points = user.Points,
                Streak = user.Streak,
                IsEmailVerified = user.IsEmailVerified,
                ReminderInterval = user.ReminderInterval,
                ReminderStartBefore = user.ReminderStartBefore,
                CreatedAt = user.CreatedAt,
                LastActive = user.LastActive,
                UserCode = user.UserCode,

                TaskItemIds = user.TaskItems
                    .Select(t => t.Id)
                    .ToList(),

                Challenges = user.UsersChallenges
                    .Select(uc => uc.ToDto())
                    .ToList()
            };
        }
    }
}