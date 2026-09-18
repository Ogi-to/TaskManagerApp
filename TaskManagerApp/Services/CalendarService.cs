using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.Exceptions;
using TaskManagerApp.Interfaces;

namespace TaskManagerApp.Services
{
    public class CalendarService : InterfacesServices.ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUserRepository _userRepository;

        public CalendarService(
            ICalendarRepository calendarRepository,
            IUserRepository userRepository)
        {
            _calendarRepository = calendarRepository;
            _userRepository = userRepository;
        }


        // ============================================
        // CREATE CALENDAR
        // ============================================

        public async Task<CalendarDto> CreateCalendarAsync(
            int userId,
            CreateCalendarDto dto)
        {
            // Проверяваме собственика
            var user = await _userRepository.GetAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            // Проверяваме името
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new Exception(
                    "Calendar name cannot be empty.");
            }

            // Проверяваме дали всички избрани приятели съществуват
            foreach (var friendId in dto.FriendIds.Distinct())
            {
                var friend =
                    await _userRepository.GetAsync(friendId);

                if (friend == null)
                {
                    throw new UserNotFoundException(friendId);
                }

                if (friendId == userId)
                {
                    throw new Exception(
                        "You cannot add yourself as a friend.");
                }
            }

            var calendar = new Calendar
            {
                Name = dto.Name.Trim(),
                OwnerId = userId,
                Color = string.IsNullOrWhiteSpace(dto.Color)
                    ? "blue"
                    : dto.Color.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            // Добавяме членовете
            foreach (var friendId in dto.FriendIds.Distinct())
            {
                calendar.Members.Add(
                    new CalendarMember
                    {
                        UserId = friendId,
                        JoinedAt = DateTime.UtcNow
                    });
            }

            await _calendarRepository.AddAsync(calendar);

            // Зареждаме календара отново
            var createdCalendar =
                await _calendarRepository.GetAsync(calendar.Id);

            if (createdCalendar == null)
            {
                throw new Exception(
                    "Calendar could not be loaded after creation.");
            }

            return MapToDto(createdCalendar);
        }


        // ============================================
        // GET USER CALENDARS
        // ============================================

        public async Task<List<CalendarDto>>
            GetUserCalendarsAsync(int userId)
        {
            var user =
                await _userRepository.GetAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            var calendars =
                await _calendarRepository
                    .GetUserCalendarsAsync(userId);

            return calendars
                .Select(MapToDto)
                .ToList();
        }


        // ============================================
        // GET SINGLE CALENDAR
        // ============================================

        public async Task<CalendarDto> GetCalendarAsync(
            int calendarId,
            int userId)
        {
            var calendar =
                await _calendarRepository
                    .GetUserCalendarAsync(
                        calendarId,
                        userId);

            if (calendar == null)
            {
                throw new Exception(
                    "Calendar does not exist or you do not have access to it.");
            }

            return MapToDto(calendar);
        }


        // ============================================
        // ADD MEMBER
        // ============================================

        public async Task AddMemberAsync(
            int userId,
            AddCalendarMemberDto dto)
        {
            // Кой прави действието?
            var owner =
                await _userRepository.GetAsync(userId);

            if (owner == null)
            {
                throw new UserNotFoundException(userId);
            }

            // Календарът трябва да съществува
            var calendar =
                await _calendarRepository
                    .GetAsync(dto.CalendarId);

            if (calendar == null)
            {
                throw new Exception(
                    "Calendar not found.");
            }

            // Само собственикът може да добавя членове
            if (calendar.OwnerId != userId)
            {
                throw new Exception(
                    "Only the calendar owner can add members.");
            }

            // Проверяваме новия потребител
            var newMember =
                await _userRepository.GetAsync(dto.UserId);

            if (newMember == null)
            {
                throw new UserNotFoundException(dto.UserId);
            }

            // Собственикът няма нужда да бъде добавян
            if (dto.UserId == calendar.OwnerId)
            {
                throw new Exception(
                    "The owner is already part of the calendar.");
            }

            // Проверяваме дали вече е член
            var alreadyMember =
                await _calendarRepository.IsMemberAsync(
                    dto.CalendarId,
                    dto.UserId);

            if (alreadyMember)
            {
                throw new Exception(
                    "This user is already a member of the calendar.");
            }

            var member = new CalendarMember
            {
                CalendarId = dto.CalendarId,
                UserId = dto.UserId,
                JoinedAt = DateTime.UtcNow
            };

            await _calendarRepository.AddMemberAsync(member);
        }


        // ============================================
        // REMOVE MEMBER
        // ============================================

        public async Task RemoveMemberAsync(
            int userId,
            int calendarId,
            int memberId)
        {
            var calendar =
                await _calendarRepository
                    .GetAsync(calendarId);

            if (calendar == null)
            {
                throw new Exception(
                    "Calendar not found.");
            }

            // Само собственикът може да премахва членове
            if (calendar.OwnerId != userId)
            {
                throw new Exception(
                    "Only the calendar owner can remove members.");
            }

            // Не можем да премахнем собственика като member
            if (memberId == calendar.OwnerId)
            {
                throw new Exception(
                    "The calendar owner cannot be removed.");
            }

            var isMember =
                await _calendarRepository.IsMemberAsync(
                    calendarId,
                    memberId);

            if (!isMember)
            {
                throw new Exception(
                    "This user is not a member of the calendar.");
            }

            await _calendarRepository.RemoveMemberAsync(
                calendarId,
                memberId);
        }


        // ============================================
        // DELETE CALENDAR
        // ============================================

        public async Task DeleteCalendarAsync(
            int userId,
            int calendarId)
        {
            var calendar =
                await _calendarRepository
                    .GetAsync(calendarId);

            if (calendar == null)
            {
                throw new Exception(
                    "Calendar not found.");
            }

            // Само собственикът може да изтрие календара
            if (calendar.OwnerId != userId)
            {
                throw new Exception(
                    "Only the calendar owner can delete the calendar.");
            }

            await _calendarRepository.DeleteAsync(
                calendarId);
        }


        // ============================================
        // MAP CALENDAR → DTO
        // ============================================

        private CalendarDto MapToDto(
            Calendar calendar)
        {
            return new CalendarDto
            {
                Id = calendar.Id,

                Name = calendar.Name,

                Color = calendar.Color,

                OwnerId = calendar.OwnerId,

                OwnerUsername =
                    calendar.Owner?.Username ?? "",

                CreatedAt = calendar.CreatedAt,

                Members = calendar.Members
                    .Select(member =>
                        new CalendarMemberDto
                        {
                            UserId = member.UserId,

                            Username =
                                member.User?.Username ?? "",

                            Email =
                                member.User?.Email ?? "",

                            JoinedAt =
                                member.JoinedAt
                        })
                    .ToList(),

                Tasks = calendar.Tasks
                    .Select(task =>
                        new CalendarTaskDto
                        {
                            Id = task.Id,

                            Name = task.Name,

                            Description =
                                task.Description,

                            StartDate =
                                task.StartDate,

                            EndDate =
                                task.EndDate,

                            State =
                                task.State,

                            Priority =
                                task.Priority,

                            UserId =
                                task.UserId,

                            CreatedByUsername =
                                task.User?.Username ?? "",

                            CalendarId =
                                calendar.Id,

                            Categories =
                                task.TasksCategories
                                    .Select(tc =>
                                        tc.Category.Name)
                                    .ToList()
                        })
                    .ToList()
            };
        }
    }
}