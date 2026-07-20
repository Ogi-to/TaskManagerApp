using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTOS
{
    public class UpdateAccountDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
