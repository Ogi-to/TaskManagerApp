using BCrypt;
using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Services
{
    public class HashPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(
               password,
               hashedPassword
            );
        }
    }
}
