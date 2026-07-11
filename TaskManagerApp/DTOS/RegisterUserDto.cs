using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTOS
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public string UserCode { get; set; } = string.Empty;

        public bool IsEmailVerified { get; set; } = false;
    }
}
