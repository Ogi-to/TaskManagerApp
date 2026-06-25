using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTOS
{
    //May not be needed, but can be used to implement a change password feature in the future
    public class ChangePasswordDto
    {
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}
