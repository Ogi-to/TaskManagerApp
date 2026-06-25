using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTOS
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
