
using TaskManagerApp.Data.Models;
using TaskManagerApp.Interfaces;
using TaskManagerApp.InterfacesRepositories;
using TaskManagerApp.InterfacesServices;
using TaskManagerApp.Exceptions;
using TaskManagerApp.DTOS;
using TaskManagerApp.DtoMappers;
using Org.BouncyCastle.Crypto.Modes.Gcm;

namespace TaskManagerApp.Services
{
    public class TasksParticipantsService : ITasksParticipantsService
    {
        private readonly ITasksParticipantsRepository _tasksParticipantsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskItemRepository _taskItemRepository;

        public TasksParticipantsService(ITasksParticipantsRepository tasksParticipantsRepository, IUserRepository userRepository,
        ITaskItemRepository taskItemRepository)
        {
            _tasksParticipantsRepository = tasksParticipantsRepository;
            _userRepository = userRepository;
            _taskItemRepository = taskItemRepository;
        }

        public async Task<List<TasksParticipantsDto>> GetByUserId(int userId)
        {
            User user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            List<TasksParticipants> tasksParticipants = await _tasksParticipantsRepository.GetByUserId(user.Id);

            if (tasksParticipants.Count == 0)
            {
                throw new UserDoesntHaveSharedTasksException();
            }

            List<TasksParticipantsDto> tasksParticipantsDtos = new List<TasksParticipantsDto>();
            foreach (var taskParticipant in tasksParticipants)
            {
                TasksParticipantsDto tasksParticipantsDto = new TasksParticipantsDto
                {
                    User = taskParticipant.User.ToDto(),
                    TaskItem = taskParticipant.TaskItem.ToDto(),
                    Status = taskParticipant.Status
                };
                tasksParticipantsDtos.Add(tasksParticipantsDto);
            }

            return tasksParticipantsDtos;
        }

        public async Task<List<TasksParticipantsDto>> GetByTaskId(int taskId)
        {
            TaskItem taskItem = await _taskItemRepository.GetAsync(taskId);
            if (taskItem == null)
            {
                throw new TaskItemNotFoundException();
            }
            List<TasksParticipants> tasksParticipants = await _tasksParticipantsRepository.GetByTaskId(taskItem.Id);

            if (tasksParticipants.Count == 0)
            {
                throw new TaskDoesntHaveSharedUsersException();
            }

            List<TasksParticipantsDto> tasksParticipantsDtos = new List<TasksParticipantsDto>();
            foreach (var taskParticipant in tasksParticipants)
            {
                TasksParticipantsDto tasksParticipantsDto = new TasksParticipantsDto
                {
                    User = taskParticipant.User.ToDto(),
                    TaskItem = taskParticipant.TaskItem.ToDto(),
                    Status = taskParticipant.Status
                };
                tasksParticipantsDtos.Add(tasksParticipantsDto);
            }

            return tasksParticipantsDtos;
        }

        public async Task<TasksParticipantsDto> GetByBoth(int taskId, int userId)
        {
            User user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            TaskItem taskItem = await _taskItemRepository.GetAsync(taskId);
            if (taskItem == null)
            {
                throw new TaskItemNotFoundException();
            }

            TasksParticipants tasksParticipants = await _tasksParticipantsRepository.GetByBoth(taskItem.Id, user.Id);
            if (tasksParticipants == null)
            {
                throw new UserNotAssignedToTaskException();
            }
            TasksParticipantsDto tasksParticipantsDto = new TasksParticipantsDto
            {
                User = tasksParticipants.User.ToDto(),
                TaskItem = tasksParticipants.TaskItem.ToDto(),
                Status = tasksParticipants.Status
            };
            return tasksParticipantsDto;
        }


        public async Task DeleteByBoth(int taskId, int userId)
        {
            User user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            TaskItem taskItem = await _taskItemRepository.GetAsync(taskId);
            if (taskItem == null)
            {
                throw new TaskItemNotFoundException();
            }

            TasksParticipants tasksParticipants = await _tasksParticipantsRepository.GetByBoth(taskItem.Id, user.Id);
            if (tasksParticipants == null)
            {
                throw new UserDoesntHaveTasksException();
            }

            await _tasksParticipantsRepository.DeleteByBoth(tasksParticipants.UserId, tasksParticipants.TaskId);
        }

        public async Task InviteFriendsToTask(int ownerId, int friendId, int taskId)
        {
            User owner = await _userRepository.GetAsync(ownerId);
            if (owner == null)
            {
                throw new UserNotFoundException(ownerId);
            }

            User friend = await _userRepository.GetAsync(friendId);
            if (friend == null)
            {
                throw new UserNotFoundException(friendId);
            }

            if (owner.Id == friend.Id)
            {
                throw new IdenticalUserIdException();
            }

            UsersRelations usersRelations = await _userRepository.GetUserRelationAsync(owner.Id, friend.Id);
            if (usersRelations == null)
            {
                throw new YouAreNotFriendsWithThisUserException();
            }

            TaskItem taskItem = await _taskItemRepository.GetAsync(taskId);
            if (taskItem == null)
            {
                throw new TaskItemNotFoundException();
            }

            if (taskItem.UserId != owner.Id)
            {
                throw new UserNotAssignedToTaskException();
            }

            if (taskItem.State != StateType.NotStarted)
            {
                throw new CantSendTaskHasEndedException();
            }

            TasksParticipants tasksParticipants = await _tasksParticipantsRepository.GetByBoth(taskItem.Id, friend.Id);
            
            if (tasksParticipants !=  null)
            {
                if (tasksParticipants.Status == Status.Pending)
                {
                    throw new InviteIsStillPendingException();
                }
                if (tasksParticipants.Status == Status.Accepted)
                {
                    throw new FriendHasAlreadyAcceptedThisTaskException();
                }
                if (tasksParticipants.Status == Status.Declined)
                {
                    throw new FriendHasDeclinedTheInvitationException();
                }
            }
            else
            {
                TasksParticipants tasksParticipants1 = new TasksParticipants
                {
                    User = friend,
                    TaskItem = taskItem,
                    Status = Status.Pending

                };
                await _tasksParticipantsRepository.Add(tasksParticipants1);

            }

        }

        public async Task AnswerInviteForTask( int friendId, int taskId, Status status)
        {
            User friend = await _userRepository.GetAsync(friendId);
            if (friend == null)
            {
                throw new UserNotFoundException(friendId);
            }

            TaskItem taskItem = await _taskItemRepository.GetAsync(taskId);
            if (taskItem == null)
            {
                throw new TaskItemNotFoundException();
            }

            if (taskItem.State != StateType.NotStarted)
            {
                throw new CantJoinTaskHasEndedException();
            }
            TasksParticipants tasksParticipants = await _tasksParticipantsRepository.GetByBoth(taskItem.Id, friend.Id);
            if (tasksParticipants == null)
            {
                throw new YouAreNotInvitedForThisTaskException();
            }

            tasksParticipants.Status = status;
            await _tasksParticipantsRepository.Update(tasksParticipants);
        }
    }
}
