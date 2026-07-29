using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;

namespace TaskManagerApp.DtoMappers
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(this TaskItem task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                CompletedAt = task.CompletedAt,
                State = task.State,
                UserId = task.UserId,
                Categories = task.TasksCategories
                    .Select(tc => tc.Category.Name)
                    .ToList()
            };
        }
    }
}
