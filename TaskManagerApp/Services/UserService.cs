using TaskManagerApp.Data.Models;
using TaskManagerApp.DtoMappers;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesRepositories;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Repositories;

namespace TaskManagerApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRankRepository _rankRepository;
        private readonly IEmailService _emailCodeService;
        private readonly HashPasswordService _hashPasswordService;
        private readonly IUserStatsService _userStatsService;
        private readonly ITaskItemRepository _taskItemRepository;

        public UserService(IUserRepository userRepository, IRankRepository rankRepository, ITaskItemRepository taskItemRepository,
            IEmailService emailCodeService, HashPasswordService hashPasswordService, IUserStatsService userStatsService)
        {
            _userRepository = userRepository;
            _rankRepository = rankRepository;
            _emailCodeService = emailCodeService;
            _hashPasswordService = hashPasswordService;
            _userStatsService = userStatsService;
            _taskItemRepository = taskItemRepository;
           
        }
        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);

            return user.ToDto();

        }

        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var testUser = await _userRepository.GetByUsernameAsync(registerUserDto.Username);
            if (testUser != null)
            {
                throw new UserNameAlreadyExistsException();
            }
            testUser = await _userRepository.GetByEmailAsync(registerUserDto.Email);
            if (testUser != null)
            {
                throw new UserEmailAlreadyExistsException();
            }

            var user = new User
            {
                Username = registerUserDto.Username,
                Email = registerUserDto.Email,
                PasswordHash = _hashPasswordService.HashPassword(registerUserDto.Password),
                UserCode = Random.Shared.Next(10000000, 99999999).ToString(),
                IsEmailVerified = false,
                LastActive = null,
            }; ;

            

            await _userRepository.CreateAccountAsync(user);
            await _userStatsService.CreateUserStatsAsync(user.Id);
            await _emailCodeService.SendVerificationCode(user.Email);
        }

        public async Task<UserDto> LogInUser(LoginUserDto loginUserDto)
        {
            var testUser = await _userRepository.GetByEmailAsync(loginUserDto.Email);
            if (testUser == null)
            {
                throw new EmailorPasswordNotFoundException();
            }

            if (testUser.IsEmailVerified == false)
            {
                throw new EmailNotVerifiedException();
            }

            var isPasswordValid = _hashPasswordService.VerifyPassword(loginUserDto.Password, testUser.PasswordHash);

            if (isPasswordValid == false)
            {
                throw new EmailorPasswordNotFoundException();
            }
            await _emailCodeService.SendVerificationCode(testUser.Email);
            var isVerified = await _emailCodeService.VerifyEmail(testUser.Email, loginUserDto.Code);
            if (!isVerified)
            {
                throw new InvalidVerificationCodeException();
            }

            return testUser.ToDto();
        }

        public async Task DeleteAccount(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            await _userRepository.DeleteAccountAsync(user.Id);
        }

        public async Task UpdateStreak(UpdateUserDto user)
        {
          
            var today = DateTime.UtcNow.Date;
            if (user.LastActive?.Date == today)
            {
                return;
            }

            if (user.LastActive?.Date == today.AddDays(-1))
            {
                user.Streak += 1;
            }
            else
            {
                user.Streak = 1;
            }

            user.LastActive = DateTime.UtcNow;
        }

        public async Task UpdateRank(UpdateUserDto user)
        {
            
            var newRank = await _rankRepository.GetRankForPoints(user.Points);
            if (newRank == null)
            {
                throw new Exception("No rank found for the given points.");
            }
            user.RankId = newRank.Id;
        }

        public async Task UpdatePoints(int userId, int points)
        {
            User user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            user.Points += points;
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                Points = user.Points,
                RankId = user.RankId,
                LastActive = user.LastActive,
                Streak = user.Streak
            };

            await UpdateRank(updateUserDto);
            await UpdateStreak(updateUserDto);
            await _userRepository.UpdateUserInfoAsync(updateUserDto, user.Id);
        }

        public async Task SendFriendRequest(int initiatorId, int relatedUserId)
        {
            User initiator = await _userRepository.GetAsync(initiatorId);
            if (initiator == null)
            {
                throw new UserNotFoundException(initiatorId);
            }
            initiator.ToDto();



            User relatedUser = await _userRepository.GetAsync(relatedUserId);
            if (relatedUser == null)
            {
                throw new UserNotFoundException(relatedUserId);
            }
            relatedUser.ToDto();
            if (initiator == relatedUser)
            {
                throw new InvalidFriendRequestException();
            }
            UsersRelations usersRelations = await _userRepository.GetUserRelationAsync(initiator.Id, relatedUser.Id);
            if (usersRelations != null)
            {
                if (usersRelations.RelationStatus == RelationStatus.Blocked)
                {
                    throw new TheUserHasBlockedYouException();
                }
                if (usersRelations.RelationStatus == RelationStatus.Pending)
                {
                    throw new InviteIsStillPendingException();
                }
                if (usersRelations.RelationStatus == RelationStatus.Accepted)
                {
                    throw new YouAreAlreadyFriendsException();
                }

            }

            int permitedIvitesForToday = 20;
            List<UsersRelations> initiatorInvitesForToday = await _userRepository.GetUserInvitesTodayAsync(initiator.Id);

            if (initiatorInvitesForToday.Count > permitedIvitesForToday)
            {
                throw new ExceededNumberOfInvitesForOneDayException(permitedIvitesForToday);
            }

            UsersRelations usersRelation = new UsersRelations
            {
                Initiator = initiator,
                InitiatorId = initiator.Id,
                RelatedUserId = relatedUser.Id,
                RelatedUser = relatedUser,

            };
            await _userRepository.SendRequestAsync(usersRelation);
            //SEND EMAIL TO THE RELATED USER!

        }

        public async Task<List<UsersRelationsDto>> GetUnansweredRelationReceivedByUserIdAsync(int relatedUserId)
        {
            User relatedUser = await _userRepository.GetAsync(relatedUserId);
            if (relatedUser == null)
            {
                throw new UserNotFoundException(relatedUserId);
            }
            List<UsersRelations> userRelations = await _userRepository.GetUnansweredRelationReceivedByUserIdAsync(relatedUserId);
            List<UsersRelationsDto> usersRelationsDtos = new List<UsersRelationsDto>();
            foreach (var userRelation in userRelations)
            {
                UsersRelationsDto userRelationsDto = new UsersRelationsDto
                {
                    Initiator = userRelation.Initiator.ToDto(),
                    RelatedUser = userRelation.RelatedUser.ToDto(),
                    RelationStatus = userRelation.RelationStatus,
                    RelationType = userRelation.RelationType,
                    CreatedAt = userRelation.CreatedAt,
                    TimeOfAction = userRelation.TimeOfAction,
                };
                usersRelationsDtos.Add(userRelationsDto);
                
            }

             return usersRelationsDtos;

        }

        public async Task AnswerToSentRequest(int relatedUserId, int userInitiatorId, RelationStatus relationStatus)
        {
            User relatedUser = await _userRepository.GetAsync(relatedUserId);
            if (relatedUser == null)
            {
                throw new UserNotFoundException(relatedUserId);
            }
            List<UsersRelations> pendingRequests = await _userRepository.GetUnansweredRelationReceivedByUserIdAsync(relatedUserId);

            User userInitiator = await _userRepository.GetAsync(userInitiatorId);
            if (userInitiator == null)
            {
                throw new UserNotFoundException(userInitiatorId);
            }
            foreach (UsersRelations userRelation in pendingRequests)
            {
                if (userRelation.InitiatorId == userInitiator.Id)
                {
                    //SEND EMAIL TO INFORM THE INITIATOR
                    if (relationStatus == RelationStatus.Accepted)
                    {
                        userRelation.RelationStatus = relationStatus;
                        userRelation.RelationType = RelationType.Friend;
                        userRelation.TimeOfAction = DateTime.UtcNow;
                    }
                    if (relationStatus == RelationStatus.Blocked)
                    {
                        userRelation.RelationStatus = relationStatus;
                        userRelation.RelationType = RelationType.Blocked;
                        userRelation.TimeOfAction = DateTime.UtcNow;
                    }
                    if (relationStatus == RelationStatus.Rejected)
                    {
                        userRelation.RelationStatus = relationStatus;
                        userRelation.RelationType = null;
                        userRelation.TimeOfAction = DateTime.UtcNow;
                    }
                    await _userRepository.RespondToRequestAsync(userRelation);
                    break;
                    
                }
            }
           

        }

        public async Task UpdateUserRelation(int initiatorId, int relatedUserId, RelationType relationType)
        {
            User user = await _userRepository.GetAsync(initiatorId);
            if (user == null)
            {
                throw new UserNotFoundException(initiatorId);
            }

            User relatedUser = await _userRepository.GetAsync(relatedUserId);
            if (relatedUser == null)
            {
                throw new UserNotFoundException(relatedUserId);
            }
            UsersRelations usersRelation = await _userRepository.GetUserRelationAsync(user.Id, relatedUser.Id);
            if (usersRelation == null)
            {
                throw new RelationNotFoundException();
            }

            if (usersRelation.RelationType == RelationType.Friend)
            {
                if (relationType == RelationType.Blocked)
                {
                    //SEND EMAIL THAT YOU HAVE BEEN BLOCKED
                    usersRelation.RelationType = relationType;
                }
                if (relationType == RelationType.Unfriend)
                {
                    //SEND EMAIL THAT YOU ARE NO LONGER FRIENDS
                    usersRelation.RelationType = relationType;
                }

            }
            if (usersRelation.RelationType == RelationType.Blocked)
            {
                if (relationType == RelationType.Friend)
                {
                    //SEND EMAIL THAT YOU HAVE BEEN UNBLOCKED
                    usersRelation.RelationType = relationType;
                }
               
            }
            if (usersRelation.RelationType == RelationType.Unfriend)
            {
                if (relationType == RelationType.Blocked)
                {
                    //SEND EMAIL THAT YOU HAVE BEEN BLOCKED
                    usersRelation.RelationType = relationType;
                }
                if (relationType == RelationType.Friend)
                {
                    //SEND EMAIL THAT YOU ARE FRIENDS
                    usersRelation.RelationType = relationType;
                }
            }
            //DOES THE SAME JOB AS AN UPDATE METHOD.
            await _userRepository.RespondToRequestAsync(usersRelation);
           

        }

        public async Task DeleteUserRelationByMoreThanAMonth()
        {
            await _userRepository.DeleteAllUnansweredUserRelationsByMoreThanAMonth();
        }


        public async Task UpdateReminderSettings(ReminderSettingsDto reminderSettingsDto)
        {
            var user = await _userRepository.GetAsync(reminderSettingsDto.Id);
            if (user == null)
            {
                throw new UserNotFoundException(reminderSettingsDto.Id);
            }
            ReminderSettingsDto reminderSettings = new ReminderSettingsDto
            {
                Id = reminderSettingsDto.Id,
                ReminderInterval = reminderSettingsDto.ReminderInterval,
                ReminderStartBefore = reminderSettingsDto.ReminderStartBefore,
            };

            reminderSettings.ReminderStartBefore = reminderSettingsDto.ReminderStartBefore;
            reminderSettings.ReminderInterval = reminderSettingsDto.ReminderInterval;
            await _userRepository.UpdateUserReminders(reminderSettings, user.Id);
        }

        public async Task SendReminderForTasksEmail()
        {
            List<TaskItem> tasks = await _taskItemRepository.GetAllAboutToStartAsync();
            Console.WriteLine(tasks.Count);
            foreach (var task in tasks)
            {

                User user = task.User;
                if (task.LastSendReminder == default || task.LastSendReminder.Value.AddMinutes(user.ReminderInterval) <= DateTime.UtcNow)
                {
                    await _emailCodeService.SendReminderTaskEmail(user.Username, user.Email, task);
                    task.LastSendReminder = DateTime.UtcNow;
                }
            }
            await _taskItemRepository.SaveChanges();


        }


        public async Task VerifyEmail(VerifyEmailDto verifyEmailDto)
        {
            var isVerified = await _emailCodeService.VerifyEmail(verifyEmailDto.Email, verifyEmailDto.Code);
            if (!isVerified)
            {
                throw new InvalidVerificationCodeException();
            }
            var user = await _userRepository.GetByEmailAsync(verifyEmailDto.Email);
            if (user == null)
            {
                throw new EmailorPasswordNotFoundException();
            }
            user.IsEmailVerified = true;
            UpdateAccountDto updateAccountDto = new UpdateAccountDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Password = user.PasswordHash,
                IsEmailVerified = user.IsEmailVerified,
            };
            
            await _userRepository.UpdateAccountInfoAsync(updateAccountDto, user.Id);
        }

        public async Task ReSendVerificationCode(string email)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new EmailorPasswordNotFoundException();
            }
            await _emailCodeService.SendVerificationCode(user.Email);
        }

        public async Task SendReminderForStreakEmail()
        {
            var today = DateTime.UtcNow.Date;
            List<User> users = await _userRepository.GetUsersWithStreaksAboutToEndAsync();
            foreach (var user in users)
            {
                if (user.LastActive > today.AddDays(-1))
                {
                    await _emailCodeService.SendReminderForStreakEmail(user);
                }
               
            }

        }
    }
}