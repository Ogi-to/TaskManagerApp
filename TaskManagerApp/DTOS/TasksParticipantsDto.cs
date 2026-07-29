using TaskManagerApp.Data.Models;

namespace TaskManagerApp.DTOS
{
    public class TasksParticipantsDto
    {
        public UserDto User { get; set; }
        public TaskDto TaskItem { get; set; }
        public Status Status{ get; set; }
    }
}
