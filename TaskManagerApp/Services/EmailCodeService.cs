using System.Net;
using System.Net.Mail;
using TaskManagerApp.Data.Models;
using TaskManagerApp.InterfacesRepositories;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class EmailCodeService : IEmailCodeService
    {
        readonly IEmailCodeRepository _emailCodeRepository;
        readonly IEmailCodeService _emailCodeService;
        public EmailCodeService(IEmailCodeRepository emailCodeRepository, IEmailCodeService emailCodeService)
        {
            _emailCodeRepository = emailCodeRepository;
            _emailCodeService = emailCodeService;
        }

        public async Task<bool> VerifyEmail(string email, string code)
        {
            var emailCode = await _emailCodeRepository.GetValidCodeAsync(email, code);

            if (emailCode == null)
                return false;

            await _emailCodeRepository.MarkAsUsedAsync(emailCode);

            return true;
        }

        public async Task SendVerificationCode(string email)
        {
            var code = new EmailCode
            {
                Email = email,
                Code = Random.Shared.Next(100000, 999999).ToString(),
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };


            await _emailCodeRepository.AddAsync(code);


            var message = new MailMessage();

            message.From = new MailAddress("oginik7@gmail.com");
            message.To.Add($"{email}");
            message.Subject = "Verification Code";
            message.Body = $"Your verification code is {code.Code}";

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }
    }
   
}
