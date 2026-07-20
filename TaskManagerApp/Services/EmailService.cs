using System.Net;
using System.Net.Mail;
using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesRepositories;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class EmailService : IEmailService
    {
        readonly IEmailCodeRepository _emailCodeRepository;
        public EmailService(IEmailCodeRepository emailCodeRepository)
        {
            _emailCodeRepository = emailCodeRepository;
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

        public async Task DeleteCodes()
        {
            await _emailCodeRepository.DeleteExpiredCodesAsync();
        }


        //TODO: BETTER EMAIL DESIGN SHOULD BE IMPLEMENTED
        public async Task SendReminderEmail(string username, string email, TaskItem taskItem)
        {
        
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.To.Add($"{email}");
            var html = await File.ReadAllTextAsync("Templates/ReminderEmail.html");

            // Replace placeholders with actual values
            html = html.Replace("{{UserName}}", username);
            html = html.Replace("{{TaskName}}", taskItem.Name);
            html = html.Replace("{{StartTime}}", taskItem.StartDate.ToString("yyyy-MM-dd HH:mm"));

            message.IsBodyHtml = true;
            message.Body = html;

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

        //TODO: BETTER EMAIL DESIGN SHOULD BE IMPLEMENTED
        public async Task SendEmailForNewChallenges(string username, string email, List<Challenge> challenges)
        {
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.To.Add($"{email}");
            var html = await File.ReadAllTextAsync("Templates/NewChallengesEmail.html");
            // Replace placeholders with actual values
            html = html.Replace("{{UserName}}", username);

            message.IsBodyHtml = true;
            message.Body = html;

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
