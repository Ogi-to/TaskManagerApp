namespace TaskManagerApp.DTOS
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
